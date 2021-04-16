using api.interfaces;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
namespace api.models
{
    public class Activity
    {
        public int activityId {get; set;}
        public string activityName {get; set;}
        public double trainerPriceForActivity {get; set;}
        public IWriteActivity writeBehavior {get; set;}
        public IDeleteActivity deleteBehavior {get; set;}
        public IReadActivity readBehavior {get; set;}        
    }
}