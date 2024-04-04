using cpu;


Task.Run(async () => {
    new SpaceInvader().Run();
}).GetAwaiter().GetResult();
