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

        public bool Compare(Notes note)
        {
            return (this.Title == note.Title && this.Text == note.Text && this.PinStat == note.PinStat
                && this.label.TrueForAll(x => x.LabelCompare(note.label))) && this.checklist.TrueForAll(x => x.CheckListCompare(note.checklist));
        }
    }

    public class Label
    {
        [Key]
        public int Label_ID { get; set; }
        public string label { get; set; }
        
    }
    public static class Extension { 

    public static bool LabelCompare(this Label label, List<Label> lab)
    {

        return (lab.TrueForAll(y => y.label == label.label));
    }
    public static  bool CheckListCompare(this CheckList list, List<CheckList> checklist)
        {
            return (checklist.TrueForAll(x => x.list == list.list));
        }
    }
    public class CheckList
    {
        [Key]
        public int Check_ID { get; set; }
        public string list { get; set; }

    }
}
