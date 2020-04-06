using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebApplication1.Models
{
    public class Exam
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "use only letters")]
        private string surname;
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "use only letters")]
        private string subject;
        [Required]
        [Range(0, 100)]
        private int grade;
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "use only letters")]
        private string teacher;
        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "use only letters")]
        private string speciality;
        
        public Exam()
        {

        }
       
       
        public string Subject
        {
            get
            { return subject; }
            set
            {
               
                    subject = value;
                
            }
        }
        public string Surname
        {
            get
            { return surname; }
            set
            {
               
                surname = value;
                
            }

        }
        public int Grade
        {
            get
            { return grade; }
            set
            {


                
                    grade = value;
                
            }
        }
        public string Teacher
        {
            get
            { return teacher; }
            set
            {
                
                    teacher = value;
                
            }
        }
        public string Speciality
        {
            get
            { return speciality; }
            set
            {
                
                    speciality = value;
                
            }
        }
      

    }
}
