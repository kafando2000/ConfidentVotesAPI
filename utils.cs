namespace ConfidentVotesAPI.utils
{
    public sealed class ArgsParser
    {
        public static bool IsTestWithSwaggerUI (string[] args)
        {   
            bool testwithswaggerui = false;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--testwithswaggerui":
                        if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool testwithswagger))
                        {
                            testwithswaggerui = testwithswagger;
                            i++; 
                        }
                        break;
                    default:
                        break;
                    
                }
            }
            Console.WriteLine($"Test With Swagger UI : {testwithswaggerui}");
            return testwithswaggerui;
        }
        public static bool IsGenerateapiclient (string[] args)
        {   
            bool Generateapiclient = false;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--generateapiclient":
                        if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool generateapiclient))
                        {
                            Generateapiclient = generateapiclient;
                            i++; 
                        }
                        break;
                    default:
                        break;
                    
                }
            }
            Console.WriteLine($"Test With Swagger UI : {Generateapiclient}");
            return Generateapiclient;
        }
    }
}


