using ConsolePaint.Terminal;

Terminal terminal = 
                    args.Length != 0 
                    ? new(args[0]) 
                    : new();

terminal.Run();


