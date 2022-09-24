class Program
{
    /// <summary>
    /// From the user input, give the table_size
    /// </summary>
    /// <param name="table_size"></param>
    static int Table_Size(out int table_size)
    {
        Console.Write("Size of the GameBoard (max 100) : ");
        string temp = Console.ReadLine()!;
        if (int.TryParse(temp, out table_size))
            if (table_size <= 100 && table_size > 0)
                return table_size;
        Console.WriteLine("Incorrect number : automatic initialization at 10");
        return table_size = 10;
    }

    /// <summary>
    /// From the user input, give the game mode
    /// </summary>
    /// <returns>0 (Game Of Life) or 1 (Day And Night)</returns>
    static int Game_Mode()
    {
        Console.Write("Choose Game Of Life (type 0) or Day And Night (type 1) : ");
        string temp = Console.ReadLine()!;
        if (int.TryParse(temp, out int result))
            if (result == 0 || result == 1)
                return result;
        Console.WriteLine("Incorrect input : automatic initialization on Game Of Life");
        return 0;
    }

    /// <summary>
    /// From the user input, give the number_of_round
    /// </summary>
    /// <param name="number_of_round"></param>
    static int Number_Of_Round(out int number_of_round)
    {
        Console.Write("Number of round : ");
        string temp = Console.ReadLine()!;
        if (int.TryParse(temp, out number_of_round))
            if (number_of_round > 0)
                return number_of_round;
        Console.WriteLine("Incorrect number : automatic initialization at 100");
        return number_of_round = 100;
    }

    /// <param name="table_size"></param>
    /// <returns>The gameboard</returns>
    static int[,] Create(int table_size)
    {
        return new int[table_size, table_size];
    }
    
    /// <summary>
    /// Randomly initialize the gameboard
    /// </summary>
    /// <param name="table"></param>
    /// <param name="table_size"></param>
    static void Init(int[,] table, int table_size)
    {
        var random = new Random();
        for (int i = 0; i < table_size; i++)
            for (int j = 0; j < table_size; j++)
                table[i, j] = random.Next(2);
    }

    /// <summary>
    /// Display the gameboard
    /// </summary>
    /// <param name="table"></param>
    /// <param name="table_size"></param>
    static void Display(int[,] table, int table_size)
    {
        Console.WriteLine();
        for (int i = 0; i < table_size; i++)
        {
            for (int j = 0; j < table_size; j++)
                Console.Write($"{table[i, j]} ");
            Console.WriteLine();
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    /// <summary>
    /// Put a cell alive
    /// </summary>
    static void Alive(ref int cell)
    {
        cell = 1;
    }

    /// <summary>
    /// Kill the cell
    /// </summary>
    /// <param name="cell"></param>
    static void Kill(ref int cell)
    {
        cell = 0;
    }

    /// <summary>
    /// Create the neighbourhood matrix of the cell indexed
    /// </summary>
    /// <param name="gameboard"></param>
    /// <param name="table_size"></param>
    /// <param name="i_index"></param>
    /// <param name="j_index"></param>
    /// <returns>The neighbourhood matrix</returns>
    static int[,] Neighbourhood_Matrix(int[,] gameboard, int table_size, int i_index, int j_index)
    {
        // Pas de test sur l'existence de la cellule dans le gameboard car cela sera gérer
        // par des clics à la souris sur les cellules directement
        var neighbourhood_matrix = new int[3, 3];
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
                // Put 0 if the index is outside the bounds of the gameboard
                if (i_index - 1 + i < 0 || i_index - 1 + i > table_size - 1
                    || j_index - 1 + j < 0 || j_index - 1 + j > table_size - 1)
                    neighbourhood_matrix[i, j] = 0;
                // Put the value of the gameboard in the neighbourhood_matrix
                else
                    neighbourhood_matrix[i, j] = gameboard[i_index - 1 + i, j_index - 1 + j];
        }

        return neighbourhood_matrix;
    }

    /// <summary>
    /// Test if the neighbourhood matrix run correctly
    /// </summary>
    /// <param name="gameboard"></param>
    /// <param name="table_size"></param>
    static void Test_Neighbourhood_Matrix(int[,] gameboard, int table_size)
    {
        // Input the index of the cell from which the neighbourhood matrix will be created
        Console.Write("i : ");
        int.TryParse(Console.ReadLine(), out int i);
        Console.Write("j : ");
        int.TryParse(Console.ReadLine(), out int j);

        // Create the neighbourhood matrix
        int[,] neighbourhood_matrix = new int[table_size, table_size];
        neighbourhood_matrix = Neighbourhood_Matrix(gameboard, table_size, i, j);

        // Display the neighbourhood matrix
        Console.WriteLine();
        Console.WriteLine("Neighbourhood Matrix :");
        Display(neighbourhood_matrix, 3);
    }
    
    /// <summary>
    /// Run a round of Game Of Life
    /// </summary>
    /// <param name="gameboard_init"></param>
    /// <param name="table_size"></param>
    static void Round_Game_Of_Life(ref int[,] gameboard_init, int table_size)
    {
        int[,] gameboard_new = Create(table_size);
        for (int i = 0; i < table_size; i++)
            for (int j = 0; j < table_size; j++)
                gameboard_new[i, j] = gameboard_init[i, j];

        int[,] neighbourhood_matrix = new int[3, 3];
        int cpt;

        for (int i = 0; i < table_size; i++)
        {
            for (int j = 0; j < table_size; j++)
            {
                neighbourhood_matrix = Neighbourhood_Matrix(gameboard_init, table_size, i, j);
                cpt = 0;
                foreach (var elem in neighbourhood_matrix)
                    cpt += elem;
                if (gameboard_init[i, j] == 1)
                    cpt--;
                if ((gameboard_init[i, j] == 1) && cpt != 2 && cpt != 3)
                    gameboard_new[i, j] = 0;
                else if ((gameboard_init[i, j] == 0) && (cpt == 3))
                    gameboard_new[i, j] = 1;
            }
        }
        gameboard_init = gameboard_new;
}
    
    /// <summary>
    /// Run a round of Day Or Night
    /// </summary>
    /// <param name="gameboard_init"></param>
    /// <param name="table_size"></param>
    static void Round_Day_And_Night(ref int[,] gameboard_init, int table_size)
    {
        int[,] gameboard_new = Create(table_size);
        for (int i = 0; i < table_size; i++)
            for (int j = 0; j < table_size; j++)
                gameboard_new[i, j] = gameboard_init[i, j];

        int[,] neighbourhood_matrix = new int[3, 3];
        int cpt;

        for (int i = 0; i < table_size; i++)
        {
            for (int j = 0; j < table_size; j++)
            {
                neighbourhood_matrix = Neighbourhood_Matrix(gameboard_init, table_size, i, j);
                cpt = 0;
                foreach (var elem in neighbourhood_matrix)
                    cpt += elem;
                if (gameboard_init[i, j] == 1)
                    cpt--;
                if ((gameboard_init[i, j] == 1) && cpt != 3 && cpt != 6 && cpt != 7 && cpt != 8)
                    gameboard_new[i, j] = 0;
                else if ((gameboard_init[i, j] == 0) && (cpt != 0 && cpt != 1 && cpt != 2 && cpt != 5 && cpt != 9))
                    gameboard_new[i, j] = 1;
            }
        }
        gameboard_init = gameboard_new;
    }

    static void Main()
    {
        // create the gameboard
        Table_Size(out int table_size);
        int[,] gameboard = Create(table_size);

        // random initialization of the gameboard
        Init(gameboard, table_size);

        // display the gameboard
        Display(gameboard, table_size);


        // test alive and kill -> put in a class
        /*
        Alive(ref gameboard[0,0]);
        Display(gameboard, table_size);
        Kill(ref gameboard[0,0]);
        Display(gameboard, table_size);
        */
        // Test_Neighbourhood_Matrix(gameboard, table_size);

        int game_mode = Game_Mode();
        Number_Of_Round(out int number_of_round);

        if (game_mode == 0)
            for (int i = 0; i < number_of_round; i++)
            {
                Console.WriteLine($"round {i + 1} !");
                Round_Game_Of_Life(ref gameboard, table_size);
                Display(gameboard, table_size);
            }
        else
            for (int i = 0; i < number_of_round; i++)
            {
                Console.WriteLine($"round {i + 1} !");
                Round_Day_And_Night(ref gameboard, table_size);
                Display(gameboard, table_size);
            }


    }
}