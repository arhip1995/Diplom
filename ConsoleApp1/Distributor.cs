﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Distributor
    {
        Enrollee[] enrollees;
        bool[] flags;
        Speciality[] specialities;
        Dictionary<Speciality, List<Enrollee>> dict;
        public int variants = 5;
        private int p;

        public Distributor(Enrollee[] _enrollees, Speciality[] _specialities)
        {
            enrollees = _enrollees;
            specialities = _specialities;
            dict = new Dictionary<Speciality, List<Enrollee>>();
            flags = new bool[_enrollees.Length];
            for (int i = 0; i<_enrollees.Length; i++)
            {
                flags[i] = false;
            }
        }

        public void distributeEnrollees()
        {
            for (int i=0; i<variants; i++)
            {
                List<Enrollee>[] studentsOrders = new List<Enrollee>[specialities.Length];
                for (int j = 0; j < enrollees.Length; j++)
                {
                    if (flags[j] == true) continue;
                    Enrollee en = enrollees[j];
                    
                    Request[] req = en.Request.ToArray();
                    Array.Sort(req, (x, y) => x.Priority.CompareTo(y.Priority));
                    Speciality sp = req[i].IdSpecialityNavigation;
                    int indexOfUniv = Array.IndexOf(specialities, sp);
                    if (studentsOrders[indexOfUniv] == null)
                    {
                        studentsOrders[indexOfUniv] = new List<Enrollee>();
                    }
                    studentsOrders[indexOfUniv].Add(en);

                }
                for (int k = 0; k< studentsOrders.Length; k++)
                {
                    Speciality curr = specialities[k];
                    List<Enrollee> orders = studentsOrders[k];
                    if (orders == null)
                    {
                        continue;
                    }
                    orders.Sort( (y, x) => x.Zno.CompareTo(y.Zno));
                    foreach (Enrollee stud in orders.ToArray())
                    {
                        Rules rule = curr.Rules.First();
                        int palces = rule.Count;
                        bool enoughScore = (rule.Score < stud.Zno) ? true : false;

                        if (dict.ContainsKey(curr) == false)
                        {
                            dict[curr] = new List<Enrollee>();
                        }
                        

                        if (enoughScore == true && dict[curr].Count < rule.Count)
                        {
                            dict[curr].Add(stud);
                            int indexOfStud = Array.IndexOf(enrollees, stud);
                            flags[indexOfStud] = true;

                        }

                    }
                }

            }
            int cc = 0;
            for (int r = 0; r<flags.Length; r++)
            {
                if (flags[r] == false)
                {
                    cc++;
                }
            }
            Console.WriteLine($" free student: {cc}");
            //распределить остольных
            List<Enrollee> freeEnrollers = new List<Enrollee>();
            
            for (int p = 0; p < flags.Length; p++)
            {
                if (flags[p] == false)
                {
                    freeEnrollers.Add(enrollees[p]);
                }
            }
            freeEnrollers.Sort((x, y) => x.Zno.CompareTo(y.Zno));

            foreach ( Enrollee enr in freeEnrollers.ToArray())
            {
                Request[] req = enr.Request.ToArray();
                Array.Sort(req, (x, y) => x.Priority.CompareTo(y.Priority));
                foreach (Request _req in req)
                {
                    Speciality spec = _req.IdSpecialityNavigation;
                    
                    Rules rule = spec.Rules.First();
                    int palces = rule.Count;
                    bool enoughScore = (rule.Score < enr.Zno) ? true : false;

                    if (enoughScore == true && dict[spec].Count < rule.Count)
                    {
                        dict[spec].Add(enr);
                        int indexOfStud = Array.IndexOf(enrollees, enr);
                        flags[indexOfStud] = true;

                    }

                }
            }
            //end


            foreach (KeyValuePair<Speciality, List<Enrollee>> entry in dict)
            {
                Console.WriteLine($"University: {entry.Key.IdDepartmentNavigation.IdUniversityNavigation.Name}\n\tSpeciality: {entry.Key.Name}");
                Console.WriteLine($"Rules: count = {entry.Key.Rules.First().Count}, students = {entry.Value.Count}");
                foreach (Enrollee stud in entry.Value.ToArray())
                {
                    Console.WriteLine($"Name: {stud.Name}, ZNO: {stud.Zno}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Free studets: ");
            int allFreeStud = 0;
            for (int p = 0; p < flags.Length;  p++)
            {
                if (flags[p] == false)
                {
                    allFreeStud++;
                    Console.WriteLine($"Name: {enrollees[p].Name}, ZNO: {enrollees[p].Zno}");
                }

            }
            Console.WriteLine($"Free studets count: {allFreeStud} ");
        }

    }
}
