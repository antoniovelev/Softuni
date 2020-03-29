namespace StudentsSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Common.Models;

    public class Exercise : BaseDeletableModel<string>
    {
        public Exercise()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [MaxLength(int.MaxValue)]
        public string Condition { get; set; }

        [Required]
        public bool IsReady { get; set; }

        [Required]
        public string HomeworkId { get; set; }

        public Homework Homework { get; set; }
    }
}
