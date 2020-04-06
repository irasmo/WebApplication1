using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class UserToExam
    {
        [Key]
        public int ID { get; set; }
        [Required]
     
        public int Exam_ID { get; set; }
        [Required]
        public int User_ID { get; set; }
    }

}
