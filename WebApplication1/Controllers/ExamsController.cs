using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{


    public class ExamsController : Controller
    {


        private readonly AppDbContext _db;

        [BindProperty]
        public Exam Exam { get; set; }
        
        public ExamsController(AppDbContext db)
        {
            _db = db;
        }

        

        public IActionResult Index()
        {


            return View();
        }
        public IActionResult Index_us()
        {
            return View();
            
        }
       
     

        public IActionResult Upsert(int? id)
        {
            Exam = new Exam();
            if (id == null)
            {
                return View(Exam);

            }
            Exam = _db.Exams.FirstOrDefault(u => u.ID == id);
            if (Exam == null)
            {
                return NotFound();
            }
            return View(Exam);
        }


        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Exam Exam)
        {
            if (GetRole() == "User")
            {
                if (Exam.ID == 0)
                {
                    _db.Exams.Add(Exam);
                    UserToExam usex = new UserToExam();
                    _db.SaveChanges();
                    int usi = GetUserID();
                    usex.User_ID = usi;
                    foreach (Exam ex in _db.Exams)
                    {
                        if (ex==Exam)
                        {
                            usex.Exam_ID = ex.ID;
                            break;
                        }

                    }
                    _db.UsersToExams.Add(usex);
                    _db.SaveChanges();

                }
                else
                {
                    _db.Exams.Update(Exam);
                    _db.SaveChanges();
                }

                
                return RedirectToAction("Index_us");
            }
            else 
            {

                 return RedirectToAction("Index");
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Exams.ToListAsync() });
        }
        [HttpGet]
        public async Task<IActionResult> GetFor()
        {
           
            int usid = GetUserID();
         
            var exams = from exam in _db.Exams
                        join useex in _db.UsersToExams on exam.ID equals useex.Exam_ID
                        where (useex.User_ID == usid)
                        select (exam);

           
            return Json(new { data = await  exams.ToListAsync() });
        }
        public int GetUserID()
        {

            string UserEmail = User.Identity.Name;
            User user = _db.Users.FirstOrDefault(u => u.Email == UserEmail);
            
            return user.ID;

        }
        public string GetRole()
        {
            string UserEmail = User.Identity.Name;
            User user = _db.Users.FirstOrDefault(u => u.Email == UserEmail);

            return user.Role;
        }




        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (GetRole() == "User")
            {
                var ExamOfDatBase = await _db.Exams.FirstOrDefaultAsync(u => u.ID == id);
                if (ExamOfDatBase == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }
                _db.Exams.Remove(ExamOfDatBase);
                foreach (UserToExam us in _db.UsersToExams)
                {
                    if (us.Exam_ID == ExamOfDatBase.ID)
                    {
                        _db.UsersToExams.Remove(us);
                    }
                }
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Deleted successfully" });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    
    }
}


