using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DevBuild.Utilities;


namespace DevBuild.ArrayLab_Generic
{
    class Program
    {
        public static List<StudentInfo> newStudents = new List<StudentInfo>();
        public static bool validData = false;

        static void Main(string[] args)
        {
            YesNoAnswer userAnswer = YesNoAnswer.AnswerNotGiven;
            uint userSelection = 0;
            string userEntry = "";
            StudentInfo tmpStudent = new StudentInfo();

            StudentInfo.BuildInitialStudentList(ref newStudents);
            
            while (true)
            {
                ProgramStart:
                DisplayStudentList();
                //since we may come back to the top of the loop, let's make sure this is set to false each time we get here 
                validData = false;
                while (!validData)
                {
                    tmpStudent = SelectStudent(out userSelection);
                    if (userSelection == newStudents.Count + 1)
                    {
                        newStudents.Add(AddStudent());
                        goto ProgramStart;
                    }
                    if (userSelection == newStudents.Count + 2)
                    {
                        AddFieldForAllStudents();
                        goto ProgramStart;
                    }
                }

                tmpStudent = newStudents[(int)userSelection - 1];
                Console.WriteLine($"Student {userSelection} is {tmpStudent.Name}");

                validData = false;
                while (!validData)
                {
                    Console.WriteLine($"What would you like to know about {tmpStudent.Name}? ");
                    Console.Write("Valid options for this student are: ");
                    foreach (string str in tmpStudent.Info.Keys)
                    {
                        Console.Write(str + " | ");
                    }
                    while (!validData)
                    {
                        userEntry = Console.ReadLine().Trim();
                        validData = FetchAndPrintData(tmpStudent, userEntry.Trim().ToLower());
                    }

                    userAnswer = UserInput.GetYesOrNoAnswer("Do you want information for another student? (y/n) ");
                    switch (userAnswer)
                    {
                        case YesNoAnswer.Yes:
                            {
                                //Let's do some cleanup before taking it from the top, making sure everything is reset before the start of the loop
                                userAnswer = YesNoAnswer.AnswerNotGiven;
                                userEntry = "";
                                userSelection = 0;
                                validData = true;
                                Console.WriteLine("\n");
                                tmpStudent = null;
                                goto ProgramStart;
                            }
                        case YesNoAnswer.No: return;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a student record based on a selection from a printed menu of students  
        /// </summary>
        /// <returns>A StudentInfo record containing student's data if student is found, null if no record found</returns>
        static StudentInfo SelectStudent(out uint selection)
        {
            try
            {
                Console.Write($"Please enter a selection from 1 to {newStudents.Count}, or {newStudents.Count + 1} to add a student: ");
                string userInput = Console.ReadLine();
                if (!uint.TryParse(userInput, out selection))
                {
                    throw new FormatException();
                }
                if (selection < 1 || selection > newStudents.Count)
                {
                    if (selection == newStudents.Count + 1 || selection == newStudents.Count + 2)
                    {
                        //these are special cases. User entered a valid number, we didn't retrieve a student,
                        //but we want to run the command to add a student to our list, or add a field for all students
                        validData = true;
                        return null;
                    }
                    else throw new IndexOutOfRangeException();
                }
                validData = true;
                userInput = "";
                return newStudents[(int)selection - 1];
            }
            catch (IndexOutOfRangeException) { Console.WriteLine("I couldn't match that number to a student."); }
            catch (FormatException) { Console.WriteLine("I didn't recognize that format."); }
            selection = 0;
            return null;
        }

        static StudentInfo AddStudent()
        {
            YesNoAnswer userAnswer = YesNoAnswer.AnswerNotGiven;
            StudentInfo tmpStudent;
            string newStudentName = "";
            string newStudentData_Label = "";
            string newStudentData_Field = "";

            //have user enter student's name
            Console.Write("Please enter new student's name: ");
            while (!Validation.ValidateInfo(InformationType.Name, newStudentName))
            {
                newStudentName = Console.ReadLine();
                if (!Validation.ValidateInfo(InformationType.Name, newStudentName))
                {
                    Console.Write("I didnt recognize that as a valid name. Please enter a valid name (1-30 characters, no spaces): ");
                }
            }
            tmpStudent = new StudentInfo(newStudentName);

            //prompt user to enter as much information as they have available about a student, and answer with "no" when they have no more info to add
            //(don't allow a user to enter an information type without an information value, and don't allow blank fields)
            while (userAnswer != YesNoAnswer.No)
            {
                userAnswer = UserInput.GetYesOrNoAnswer($"Would you like to enter more information about {tmpStudent.Name}? (y/n) ");
                switch (userAnswer)
                {
                    case YesNoAnswer.Yes:
                        {
                            newStudentData_Label = "";
                            newStudentData_Field = "";

                            tmpStudent.AddField(newStudentData_Label, newStudentData_Field);
                            break;
                        }
                    case YesNoAnswer.No: break;
                }
            }
            return tmpStudent;
        }

        static void DisplayStudentList()
        {
            newStudents.Sort();
            Console.WriteLine("Welcome to our C# class. Which student would you like to learn more about?");
            for (int i = 0; i <= newStudents.Count + 1; i++)
            {
                if (i == newStudents.Count)
                {
                    Console.WriteLine($"{i + 1}) (Add Student)");
                }
                else if (i == newStudents.Count + 1)
                {
                    Console.WriteLine($"{i + 1}) (Add Field for all students)");
                }
                else
                {
                    Console.WriteLine($"{i + 1}) " + newStudents[i].Name);
                }
            }
        }

        static bool FetchAndPrintData(StudentInfo studentInfo, string requestedData)
        {
            try
            {
                if (studentInfo.Info.ContainsKey(requestedData))
                {
                    validData = true;
                    Console.WriteLine($"{ studentInfo.Name }'s { requestedData } is { studentInfo.Info[requestedData] }");
                }
                
                else throw new KeyNotFoundException();
                return validData;
            }
            catch (KeyNotFoundException)
            {
                validData = false;
                Console.WriteLine("I didn't recognize that entry.");
                Console.Write($"What would you like to know about {studentInfo.Name}? ");
                return validData;
            }
        }

        static void AddFieldForAllStudents()
        {
            string label = ""; 
            string field = "";

            UserInput.PromptUntilValidEntry("What kind of information would you like to add for all students? (e.g. favorite ice cream): ", ref label);

            foreach (StudentInfo s in newStudents)
            {
                s.AddField(label, field);
            }
        }
    }
}
