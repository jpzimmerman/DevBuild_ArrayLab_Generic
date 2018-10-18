using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DevBuild.Utilities;

namespace DevBuild.ArrayLab_Generic
{
    class StudentInfo : IComparable
    {
        public string Name { get; set; }
        public Dictionary<string, object> Info { get; } = new Dictionary<string, object>();

        public StudentInfo() { }

        public StudentInfo(string studentName, params KeyValuePair<string, object>[] studentInfo)
        {
            Name = studentName;
            Info.Add("name", studentName);
            foreach (KeyValuePair<string, object> c in studentInfo)
            {
                Info.Add(c.Key, c.Value);
            }
        }

        /// <summary>
        /// This method will allow a user to add a new piece of information for a specific student
        /// </summary>
        /// <param name="informationType">The kind of information to be added for a student record (e.g "Grade", "Favorite Color", or "Days Absent" </param>
        /// <param name="informationValue">The value of the information being added (e.g "A" or "blue" or 4.)</param>
        public void AddInfo(string informationType, object informationValue)
        {
            if (!Info.ContainsKey(informationType))
            {
                Info.Add(informationType, informationValue);
            }
        }

        public void AddInfo(KeyValuePair<string, object> newInfo)
        {
            if (Info.ContainsKey(newInfo.Key))
            {
                Info.Add(newInfo.Key, newInfo.Value);
            }
        }

        /// <summary>
        /// This method will fill our student list with a selection of initial values
        /// </summary>
        /// <param name="newStudents">A list of StudentInfo objects to contain the new data</param>
        public static void BuildInitialStudentList(ref List<StudentInfo> newStudents)
        {
            //we intend this function to build an initial list; make sure this list is empty before adding these values
            if (newStudents.Count == 0)
            {
                StudentInfo studentInfo1 = new StudentInfo("Bradley",   new KeyValuePair<string, object>("hometown", "Ferndale, MI"),
                                                                        new KeyValuePair<string, object>("age", 28),
                                                                        new KeyValuePair<string, object>("favorite food", "Tacos"));

                StudentInfo studentInfo2 = new StudentInfo("Kate",  new KeyValuePair<string, object>("hometown", "Oswego, MI"),
                                                                    new KeyValuePair<string, object>("age", 26),
                                                                    new KeyValuePair<string, object>("favorite food", "Coney dogs"));

                StudentInfo studentInfo3 = new StudentInfo("Brock", new KeyValuePair<string, object>("hometown", "Dearborn, MI"),
                                                                    new KeyValuePair<string, object>("age", 32),
                                                                    new KeyValuePair<string, object>("favorite food", "Pasta"),
                                                                    new KeyValuePair<string, object>("height", "168"));

                StudentInfo studentInfo4 = new StudentInfo("Jennifer",  new KeyValuePair<string, object>("hometown", "Streetsboro, OH"),
                                                                        new KeyValuePair<string, object>("age", 23),
                                                                        new KeyValuePair<string, object>("favorite food", "Nachos"));

                StudentInfo studentInfo5 = new StudentInfo("Kayla",     new KeyValuePair<string, object>("hometown", "Flagstaff, AZ"),
                                                                        new KeyValuePair<string, object>("age", 27),
                                                                        new KeyValuePair<string, object>("hobby", "hang gliding"),
                                                                        new KeyValuePair<string, object>("favorite food", "Taquitos"));

                newStudents.Add(studentInfo1);
                newStudents.Add(studentInfo2);
                newStudents.Add(studentInfo3);
                newStudents.Add(studentInfo4);
                newStudents.Add(studentInfo5);
            }
        }

        public void AddField(string label, string field)
        {
            //newStudentData_Label = "";
            //newStudentData_Field = "";

            //stay in a loop until user enters a valid data label
            while (string.IsNullOrEmpty(label))
            {
                UserInput.PromptUntilValidEntry($"Enter the type of information you'd like to add about {Name} (e.g. favorite bird): ", ref label);
            }

            if (!Info.ContainsKey(label))
            {
                //stay in a loop until the user enters a valid data field
                UserInput.PromptUntilValidEntry($"Enter {Name}'s {label}: ", ref field);
                Info.Add(label, field);
            }
            else
            {
                Console.WriteLine("We already have that information here.");
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is StudentInfo)
            {
                return Name.CompareTo((obj as StudentInfo).Name);
            }
            else return -1;
        }
    }
}
