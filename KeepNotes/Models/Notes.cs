using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KeepNotes.Models
{
    public class Notes
    {
        public int ID { get; set; }
        public string  Title { get; set; }
        public string Text { get; set; }
        public bool PinStat { get; set; }

        public List<Label> label { get; set; }
        public List<CheckList> checklist { get; set; }
    }

    public class Label
    {
        [Key]
        public int Label_ID { get; set; }
        public string label { get; set; }
    }
    public class CheckList
    {
        [Key]
        public int Check_ID { get; set; }
        public string list { get; set; }
    }
}
