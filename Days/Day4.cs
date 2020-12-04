using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day4
    {
        public static string inputFilePath = @"C:\Users\Shaun\Documents\Projects\AdventOfCode\AdventOfCode2020\AdventOfCode2020\Inputs\inputDay4.txt";
        public static string[] testInput = new string[]
        {
            "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd",
            "byr:1937 iyr:2017 cid:147 hgt:183cm",
            "",
            "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884",
            "hcl:#cfa07d byr:1929",
            "",
            "hcl:#ae17e1 iyr:2013",
            "eyr:2024",
            "ecl:brn pid:760753108 byr:1931",
            "hgt:179cm",
            "",
            "hcl:#cfa07d eyr:2025 pid:166559648",
            "iyr:2011 ecl:brn hgt:59in",
        };

        public static string testInput_Valid = @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";


        public static string testInput_Invalid = @"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007";

        public static void Part1()
        {
            var inputStrings = File.ReadAllLines(inputFilePath);
            //var inputStrings = testInput;
            //var inputStrings = testInput_Valid.Replace("\r", "").Split('\n');
            //var inputStrings = testInput_Invalid.Replace("\r", "").Split('\n');

            var passports = new List<Passport>();
            var currentPassport = new Passport();
            var validPassports = 0;
            foreach (var inputString in inputStrings)
            {
                if(inputString == "")
                {
                    //new passport
                    passports.Add(currentPassport);
                    currentPassport = new Passport();
                    continue;
                }
                currentPassport.ParseLine(inputString);
            }
            passports.Add(currentPassport);

            foreach (var passport in passports)
            {
                if(passport.IsValid2())
                {
                    validPassports++;
                    Console.WriteLine($"  VALID: {passport.ToString()}");
                }
                else
                {
                    Console.WriteLine($"INVALID: {passport.ToString()}");
                }
            }

            Console.WriteLine($"Valid passports: {validPassports}");
        }

        public class Passport
        {
            public Dictionary<string, string> passportFields = new Dictionary<string, string>();

            /*
            Passport Fields:
                byr (Birth Year)
                iyr (Issue Year)
                eyr (Expiration Year)
                hgt (Height)
                hcl (Hair Color)
                ecl (Eye Color)
                pid (Passport ID)
                cid (Country ID)
            */

            public Dictionary<string, string> passportFieldDescriptions = new Dictionary<string, string>()
            {
                {"byr", "(Birth Year)" },
                {"iyr", "(Issue Year)" },
                {"eyr", "(Expiration Year)" },
                {"hgt", "(Height)" },
                {"hcl", "(Hair Color)" },
                {"ecl", "(Eye Color)" },
                {"pid", "(Passport ID)" },
                {"cid", "(Country ID)" },
            };

            public void ParseLine(string line)
            {
                var fields = line.Split(' ');
                foreach (var field in fields)
                {
                    var keyValue = field.Split(':');
                    passportFields[keyValue[0]] = keyValue[1];
                }
            }

            public bool IsValid1()
            {
                foreach (var field in passportFieldDescriptions)
                {
                    //Remember to skip cid:
                    if (field.Key == "cid")
                    {
                        continue;
                    }
                    if(!passportFields.ContainsKey(field.Key))
                    {
                        return false;
                    }
                }
                return true;
            }




            //Validity:
            /*
byr (Birth Year) - four digits; at least 1920 and at most 2002.
iyr (Issue Year) - four digits; at least 2010 and at most 2020.
eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
hgt (Height) - a number followed by either cm or in:
If cm, the number must be at least 150 and at most 193.
If in, the number must be at least 59 and at most 76.
hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
pid (Passport ID) - a nine-digit number, including leading zeroes.
cid (Country ID) - ignored, missing or not.
             */
            public bool IsValid2()
            {
                //Needs to have all fields:


                //And they need to be valid:
                foreach (var field in passportFieldDescriptions)
                {
                    //Remember to skip cid:
                    if (field.Key == "cid")
                    {
                        continue;
                    }
                    if (!passportFields.ContainsKey(field.Key))
                    {
                        passportFieldDescriptions[field.Key] = "[NOT ENOUGH FIELDS!]";
                        return false;
                    }
                    var passportValue = passportFields[field.Key];
                    switch (field.Key)
                    {
                        case "byr":
                            if (passportValue.Length != 4)
                            {
                                passportFieldDescriptions[field.Key] = "[NOT 4 DIGITS!]";
                                return false;
                            }
                            if (!IsValidNumberInRange(passportValue, 1920, 2002))
                            {
                                passportFieldDescriptions[field.Key] = "[NOT IN RANGE!]";
                                return false;
                            }
                            break;
                        case "iyr":
                            if (passportValue.Length != 4)
                            {
                                passportFieldDescriptions[field.Key] = "[NOT 4 DIGITS!]";
                                return false;
                            }
                            if (!IsValidNumberInRange(passportValue, 2010, 2020))
                            {
                                passportFieldDescriptions[field.Key] = "[NOT IN RANGE!]";
                                return false;
                            }
                            break;
                        case "eyr":
                            if (passportValue.Length != 4)
                            {
                                passportFieldDescriptions[field.Key] = "[NOT 4 DIGITS!]";
                                return false;
                            }
                            if (!IsValidNumberInRange(passportValue, 2020, 2030))
                            {
                                passportFieldDescriptions[field.Key] = "[NOT IN RANGE!]";
                                return false;
                            }
                            break;
                        case "hgt":
                            //hgt(Height) - a number followed by either cm or in:
                            //If cm, the number must be at least 150 and at most 193.
                            //If in, the number must be at least 59 and at most 76.
                            if (passportValue.Length < 3)
                            {
                                passportFieldDescriptions[field.Key] = "[NOT LONG ENOUGH!]";
                                return false;
                            }
                            var units = passportValue.Substring(passportValue.Length - 2);
                            var amount = passportValue.Substring(0, passportValue.Length - 2);
                            switch (units)
                            {
                                case "cm":
                                    if (!IsValidNumberInRange(amount, 150, 193))
                                    {
                                        passportFieldDescriptions[field.Key] = "[NOT IN CM RANGE!]";
                                        return false;
                                    }
                                    break;
                                case "in":
                                    if (!IsValidNumberInRange(amount, 59, 76))
                                    {
                                        passportFieldDescriptions[field.Key] = "[NOT IN IN RANGE!]";
                                        return false;
                                    }
                                    break;
                                default:
                                    {
                                        passportFieldDescriptions[field.Key] = "[WHAT IS THIS?]";
                                        return false;
                                    }
                            }
                            break;
                        //hcl(Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                        case "hcl":
                            //#[xX][0-9a-fA-F]+
                            var hairColourRegex = new Regex(@"^#[0-9a-fA-F]{6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            if (!hairColourRegex.IsMatch(passportValue))
                            {
                                passportFieldDescriptions[field.Key] = "[DID NOT MATCH REGEX]";
                                return false;
                            }
                            break;

                        //ecl(Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                        case "ecl":
                            var validEyeColours = new string[] { 
                                "amb",
                                "blu",
                                "brn",
                                "gry",
                                "grn",
                                "hzl",
                                "oth",
                            };
                            if (!validEyeColours.Contains(passportValue))
                            {
                                passportFieldDescriptions[field.Key] = "[INVALID EYE COLOUR]";
                                return false;
                            }
                            break;
                        //pid(Passport ID) - a nine - digit number, including leading zeroes.
                        case "pid":
                            var passpordIdRegex = new Regex(@"^[0-9]{9}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            if (!passpordIdRegex.IsMatch(passportValue))
                            {
                                passportFieldDescriptions[field.Key] = "[ID WRONG]";
                                return false;
                            }
                            break;
                        case "cid": 
                            //Ignored!
                            break;
                        default:
                            break;
                    }
                }
                return true;
            }

            private static bool IsValidNumberInRange(string passportValue, int minValue, int maxValue)
            {
                var parseSuccess = int.TryParse(passportValue, out var passportInt);
                if (!parseSuccess)
                    return false;
                if (passportInt < minValue)
                    return false;
                if (passportInt > maxValue)
                    return false;
                return true;
            }


            public override string ToString()
            {
                var str = "";
                foreach (var passportFieldDescription in passportFieldDescriptions)
                {
                    str += $"{passportFieldDescription.Key}:";
                    if (passportFields.ContainsKey(passportFieldDescription.Key))
                    {
                        var passportValue = passportFields[passportFieldDescription.Key];
                        str += passportValue;
                    }
                    else
                    {
                        str += "NONE";
                    }
                    str += $"{passportFieldDescriptions[passportFieldDescription.Key]}";
                    str += " ";
                }
                return str;
            }
        }
    }

}
