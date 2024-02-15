using Swed64;

// init swed
Swed swed = new Swed("cs2");

//get client.dll
IntPtr client = swed.GetModuleBase("client.dll");

//offsets.cs
int dwEntityList = 0x18B3018;

//client.dll.cs
int m_hPlayerPawn = 0x7E4;
int m_flDetectedByEnemySensorTime = 0x1440;

//glow

while (true)
{
    // entity list
    IntPtr entityList = swed.ReadPointer(client, dwEntityList);

    //first entry
    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);

    for (int i = 0; i < 64; i++)
    {
        if (listEntry == IntPtr.Zero)
            continue;

        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

        if (currentController == IntPtr.Zero)
            continue;

        int pawnHandle = swed.ReadInt(currentController, m_hPlayerPawn);
        if (pawnHandle == 0)
            continue;

        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);

        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

        swed.WriteFloat(currentPawn, m_flDetectedByEnemySensorTime, 86400);
    }

    Thread.Sleep(50);
    Console.Clear();
}