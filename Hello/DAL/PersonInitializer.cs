using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using Hello.Models;

namespace Hello.DAL
{

    public class PersonInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<PersonContext>
    {
        protected override void Seed(PersonContext context)
        {
            var persons = new List<Person>
            {
                new Person{FirstMidName="Carson",LastName="Alexander", Address="111", Age=27, Interest="ski" },
                new Person{FirstMidName="Meredith",LastName="Alonso", Address="111", Age=65, Interest="ski"},
                new Person{FirstMidName="Arturo",LastName="Anand", Address="111", Age=19, Interest="ski"},
                new Person{FirstMidName="Gytis",LastName="Barzdukas", Address="111", Age=44, Interest="ski"},
            };

            persons.ForEach(s => context.Persons.Add(s));
            context.SaveChanges();
        }
    }
}