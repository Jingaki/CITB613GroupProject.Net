using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupProjectDepositCatalogWebApp.Data;
using GroupProjectDepositCatalogWebApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GroupProjectDepositCatalogWebApp.Controllers
{
    public class DepositModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepositModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DepositModels
        public async Task<IActionResult> Index()
        {
            //var users_table = _context.Users.ToList();

            IEnumerable<SearchResultEntity> data = await (
                        from users_table in _context.Users
                        join depostis_table in _context.Deposits on users_table.Id equals depostis_table.MyApplicationUserId
                        select new SearchResultEntity
                        {
                            IdentifyingNum = depostis_table.Id,
                            Username = users_table.UserName == null ? users_table.Email : users_table.UserName,//to register you must fill the email field username is normally set to be == to email
                            DepositName = depostis_table.DepositName,
                            TypeOfCurrency = depostis_table.CurrencyType,
                            EffectiveAnnualInterestRate = depostis_table.EffectiveAnnualInterestRate,
                            MinSum = depostis_table.MinSum,
                            TypeOfInterest = depostis_table.TypeOfInterest
                        }).ToListAsync();
            if (data == null)
            {
                return NotFound();
            }
            SearchFormEntity data2send = new SearchFormEntity();
            data2send.Results = data;
            /*var applicationDbContext = _context.Deposits.Include(d => d.MyApplicationUser);
            return View(await applicationDbContext.ToListAsync());*/
            //default data
            data2send.InvestmentSum = 1000;
            data2send.DurrationPlanned = 12;

            return View(data2send);
        }

        // GET: Deposits/ShowSearchResult
        [HttpPost]
        public async Task<IActionResult> Index(SearchFormEntity SearchRequest)
        {
            if (_context.Deposits == null || _context.Users == null)
            {
                return NotFound();
            }
            bool TimeDepositValueTemp;
            bool OverdraftValueTemp;
            bool CreditValueTemp;
            if (SearchRequest.TimeDeposit == TimeDepositOption.True)
            {
                TimeDepositValueTemp = true;
            }
            else TimeDepositValueTemp = false;

            if (SearchRequest.OverdraftPossability == OverdraftPossabilityOption.True)
            {
                OverdraftValueTemp = true;
            }
            else OverdraftValueTemp = false;

            if (SearchRequest.CreditPossability == CreditPossabilityOption.True)
            {
                CreditValueTemp = true;
            }
            else CreditValueTemp = false;

            IEnumerable<SearchResultEntity> data = await (
                        from users_table in _context.Users
                        join depostis_table in _context.Deposits on users_table.Id equals depostis_table.MyApplicationUserId
                        where
                        SearchRequest.CurrencyType != null ? depostis_table.CurrencyType.Equals(SearchRequest.CurrencyType) : 1 == 1 &&
                        SearchRequest.OwnershipType != null ? depostis_table.OwnershipType.Equals(SearchRequest.OwnershipType) : 1 == 1 &&
                        SearchRequest.InterestPaymentType != null ? depostis_table.InterestPaymentType.Equals(SearchRequest.InterestPaymentType) : 1 == 1 &&
                        depostis_table.TypeOfDeposit.Equals(SearchRequest.TypeOfDeposit)//the only deposit that doesnt need a check if it is equal to 0
                        where//after too many conditions in the where it stops working dotnet :^)
                        SearchRequest.TypeOfInterest != null ? depostis_table.TypeOfInterest.Equals(SearchRequest.TypeOfInterest) : 1 == 1 &&
                        SearchRequest.TimeDeposit != TimeDepositOption.All ? depostis_table.TimeDeposit == TimeDepositValueTemp : 1 == 1
                        where
                        SearchRequest.OverdraftPossability != OverdraftPossabilityOption.All ? depostis_table.OverdraftPossability == OverdraftValueTemp : 1 == 1 &&
                        SearchRequest.CreditPossability != CreditPossabilityOption.All ? depostis_table.CreditPossability == CreditValueTemp : 1 == 1
                        where
                        depostis_table.MinSum <= SearchRequest.InvestmentSum && depostis_table.MaxSum >= SearchRequest.InvestmentSum &&
                        depostis_table.MaxDuration >= SearchRequest.DurrationPlanned && depostis_table.MinDuration <= SearchRequest.DurrationPlanned
                        //SearchRequest.DurrationPlanned <= depostis_table.MaxDuration && SearchRequest.DurrationPlanned >= depostis_table.MinDuration &&
                        // how is that different then the one above dotnet you are crazy
                        select new SearchResultEntity
                        {
                            IdentifyingNum = depostis_table.Id,
                            Username = users_table.UserName,//== null ? users_table.Email : users_table.UserName,//to register you must fill the email field username is normally set to be == to email
                            DepositName = depostis_table.DepositName,
                            TypeOfCurrency = depostis_table.CurrencyType,
                            EffectiveAnnualInterestRate = depostis_table.EffectiveAnnualInterestRate,
                            MinSum = depostis_table.MinSum,
                            TypeOfInterest = depostis_table.TypeOfInterest
                        })
                        .ToListAsync();

            SearchFormEntity data2send = SearchRequest;
            data2send.Results = data;

            return _context.Deposits != null & _context.Users != null ?
                        View("Index", data2send) :
                        Problem("Entity set 'ApplicationDbContext.Deposit'  is null.");
        }
        // GET: DepositModels/CleanUp/id for the deposit& sum of the deposit
        public async Task<IActionResult> CleanUp(int? id, float? SearchSum, int? Durration)
        //another model must be created for this calculator
        {
            //2.2.24 missing functionality not very well done
            if (id == null || _context.Deposits == null || SearchSum == null || Durration == null)
            {
                return NotFound();
            }
            var depositModel = await _context.Deposits
                .Include(d => d.ShiftingInteresData)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (depositModel == null || depositModel.MinSum > SearchSum || depositModel.MaxSum < SearchSum
                || depositModel.MinDuration > Durration || depositModel.MaxDuration < Durration)
            {
                return NotFound();
            }
            CalculatorEntity data = new CalculatorEntity();
            data.productId = (int)id;
            data.InvestmentSum = (float)SearchSum;
            data.DurrationInMonths = (int)Durration;
            if (depositModel.TypeOfInterest == InterestEnumType.Shifting)
            {
                if(depositModel.ShiftingInteresData==null) return NotFound();

                ShiftingInterestRateDataModel dataToSend = new ShiftingInterestRateDataModel();
                dataToSend.Month = -1;
                foreach(var item in depositModel.ShiftingInteresData)
                {
                    if (item.Month > dataToSend.Month && item.Month <= Durration)
                    {
                        dataToSend = item;
                    }
                }
                if (dataToSend.Month == -1) return NotFound();

                data.EffectiveYearlyInterest = dataToSend.YearlyEIR;
            }
            else
            {
                data.EffectiveYearlyInterest = depositModel.EffectiveAnnualInterestRate;
            }
            int YearlyCal = data.DurrationInMonths / 12;
            data.Total = (float)(Math.Pow((100 + data.EffectiveYearlyInterest) / 100, (float)YearlyCal)
                    * (float)SearchSum);
            data.InterestSum = data.Total - data.InvestmentSum;
            

            //repeated code
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CleanUp(CalculatorEntity calculator)
        {
            var depositModel = await _context.Deposits
                .Include(d => d.ShiftingInteresData)
                .FirstOrDefaultAsync(d => d.Id == calculator.productId);
            if (depositModel == null)
            {
                return NotFound();
            }
            if (depositModel.TypeOfInterest == InterestEnumType.Shifting)
            {
                if (depositModel.ShiftingInteresData == null) return NotFound();

                ShiftingInterestRateDataModel dataToSend = new ShiftingInterestRateDataModel();
                dataToSend.Month = -1;
                foreach (var item in depositModel.ShiftingInteresData)
                {
                    if (item.Month > dataToSend.Month && item.Month <= calculator.DurrationInMonths)
                    {
                        dataToSend = item;
                    }
                }
                if (dataToSend.Month == -1) return NotFound();

                calculator.EffectiveYearlyInterest = dataToSend.YearlyEIR;
            }
            else
            {
                calculator.EffectiveYearlyInterest = depositModel.EffectiveAnnualInterestRate;
            }
            int YearlyCal = calculator.DurrationInMonths / 12;
            calculator.Total = (float)(Math.Pow((100 + calculator.EffectiveYearlyInterest) / 100, (float)(YearlyCal))
                    * (float)calculator.InvestmentSum);
            calculator.InterestSum = calculator.Total - calculator.InvestmentSum;
            //repeated code
            return View(calculator);
        }

        // GET: DepositModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }
            var depositModel = _context.Deposits
                .Include(d => d.ShiftingInteresData)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (depositModel == null)
            {
                return NotFound();
            }

            return View(await depositModel);
        }

        // GET: DepositModels/Create
        [Authorize]
        public IActionResult Create()
        {
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: DepositModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepositModel depositModel)
        {
            ModelState.Remove("MyApplicationUser");
            ModelState.Remove("ShiftingInteresData");
            if (ModelState.IsValid)
            {
                _context.Add(depositModel);
                await _context.SaveChangesAsync();

                if (depositModel.TypeOfInterest == InterestEnumType.Shifting)
                {
                    IEnumerable<ShiftingInterestRateDataModel> data = await
                        GetDataSIRDasync(depositModel.Id);
                    if (data == null)
                    {
                        return NotFound();
                    }
                    SIRentity data2send = new SIRentity();
                    data2send.ResultsData = data;

                    ShiftingInterestRateDataModel tempData = new ShiftingInterestRateDataModel();
                    tempData.Deposit = depositModel;
                    tempData.DepositId = depositModel.Id;
                    data2send.ModelData = tempData;

                    return View("SIR", data2send);
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", depositModel.MyApplicationUserId);
            return View(depositModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSIRD(SIRentity model)
        {
            if (model.ModelData == null)
            {
                return View("SIR", model);
            }
            _context.ShiftingInterests_table.Add(model.ModelData);
            await _context.SaveChangesAsync();
            //_context.Entry(model.ModelData).Reload();

            DepositModel depositTemp = new DepositModel();
            IEnumerable<ShiftingInterestRateDataModel> data = await
                GetDataSIRDasync(model.ModelData.DepositId);
            if (data == null)
            {
                return NotFound();
            }
            model.ResultsData = data;


            return View("SIR", model);

        }

        public async Task<IActionResult> LookIntoSIRD(int depositId)
        {
            if (depositId == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var depositModel = await _context.Deposits.FindAsync(depositId);
            if (depositModel.TypeOfInterest == InterestEnumType.Fixed)
            {
                return NotFound();
            }

            IEnumerable<ShiftingInterestRateDataModel> data = await
                GetDataSIRDasync(depositId);
            if (data == null)
            {
                return NotFound();
            }
            SIRentity data2send = new SIRentity();
            data2send.ResultsData = data;

            ShiftingInterestRateDataModel tempData = new ShiftingInterestRateDataModel();
            tempData.Deposit = depositModel;
            tempData.DepositId = depositModel.Id;
            data2send.ModelData = tempData;

            return View("SIR", data2send);
        }



        // GET: DepositModels/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var depositModel = await _context.Deposits.FindAsync(id);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (depositModel == null || userId != depositModel.MyApplicationUserId)
            {
                return NotFound();
            }
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", depositModel.MyApplicationUserId);
            return View(depositModel);
        }

        // POST: DepositModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepositName,TypeOfDeposit,TypeOfInterest,TimeDeposit,OverdraftPossability,CreditPossability,MonthlyCompounding,TerminalCapitalization,ValidForClientsOnly,CurrencyType,InterestPaymentType,OwnershipType,EffectiveAnnualInterestRate,WebLinkToOffer,DescriptionOfNegotiatedInterestRate,MinSum,MinSumDescription,MinDuration,MaxSum,MaxSumDescription,MaxDuration,DurationDescription,MyApplicationUserId")] DepositModel depositModel)
        {
            if (id != depositModel.Id)
            {
                return NotFound();
            }
            ModelState.Remove("MyApplicationUser");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(depositModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepositModelExists(depositModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MyApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", depositModel.MyApplicationUserId);
            return View(depositModel);
        }


        public async Task<IActionResult> DeleteSIRD(int id)//must be named the same way as the view ???so annoying but doesnt establishes direct connection
        {

            if (_context.ShiftingInterests_table == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShiftingInterests_table'  is null.");
            }

            var SIRmodel = await _context.ShiftingInterests_table.FindAsync(id);
            if (SIRmodel != null)
            {
                _context.ShiftingInterests_table.Remove(SIRmodel);
            }

            await _context.SaveChangesAsync();

            
            IEnumerable<ShiftingInterestRateDataModel> data = await
                GetDataSIRDasync(SIRmodel.DepositId);
            if (data == null)
            {
                return NotFound();
            }
            SIRentity data2send = new SIRentity();
            data2send.ResultsData = data;


            data2send.ModelData = SIRmodel;

            return View("SIR", data2send);
            //return RedirectToAction(nameof(Index));*/
        }

        // GET: DepositModels/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deposits == null)
            {
                return NotFound();
            }

            var depositModel = await _context.Deposits
                //.Include(d => d.MyApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (depositModel == null || userId != depositModel.MyApplicationUserId)
            {
                return NotFound();
            }

            return View(depositModel);
        }

        // POST: DepositModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Deposits == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Deposits'  is null.");
            }
            var depositModel = await _context.Deposits.FindAsync(id);
            if (depositModel != null)
            {
                _context.Deposits.Remove(depositModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepositModelExists(int id)
        {
            return (_context.Deposits?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<IEnumerable<ShiftingInterestRateDataModel>> GetDataSIRDasync(int id)
        {
            return await (
                from deposit_table in _context.Deposits
                join sir_table in _context.ShiftingInterests_table
                on deposit_table.Id equals sir_table.DepositId
                where deposit_table.Id == id
                select new ShiftingInterestRateDataModel
                {
                    Id = sir_table.Id,
                    Month = sir_table.Month,
                    YearlyEIR = sir_table.YearlyEIR,
                    DepositId = sir_table.DepositId,
                    Deposit = sir_table.Deposit
                }).ToListAsync();
        }
    }
}
