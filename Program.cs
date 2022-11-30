using System;
using System.Collections.Generic;

namespace TAUT_Tautology_SPOJ
{
    public enum SymbolType
    {
        Operator,
        Variable
    }
    public class Tree
    {
        public char node;
        public SymbolType type;
        public Tree left, right;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                if (Solve.Test(Console.ReadLine()))
                    Console.WriteLine("YES");
                else
                    Console.WriteLine("NO");
            }
        }
    }
    public class Solve
    {
        private string P;
        private Tree Drzewop;
        char[] Zmiany = new char[26];
        int Licznik = 0;
        private char[] Operacje = new char[] { 'I', 'C', 'D', 'N', 'E' };
        private Solve(string p)
        {
            this.P = p;
        }
        public static bool Test(string p)
        {
            Solve ex = new Solve(p);
            int end = 0;
            ex.GenerujZast();
            ex.Drzewop = ex.AnulujZast(0, out end);
            return ex.WeryfikujKombinacje();
        }
        private bool WeryfikujKombinacje()
        {
            long limit = 1 << Licznik;
            for (int i = 0; i < limit; i++)
            {
                if (Sprawdz(i, Drzewop))
                    continue;
                return false;
            }
            return true;
        }
        private bool Sprawdz(int i, Tree t)
        {
            if (t.type == SymbolType.Variable)
                return (i & (1 << (t.node - 'a'))) != 0;
            else
            {
                if (t.node == 'N')
                    return !Sprawdz(i, t.left);
                if (t.node == 'C')
                    if (Sprawdz(i, t.left))
                        return Sprawdz(i, t.right);
                    else
                        return false;
                if (t.node == 'D')
                    if (!Sprawdz(i, t.left))
                        return Sprawdz(i, t.right);
                    else
                        return true;
                if (t.node == 'I')
                {
                    bool left = Sprawdz(i, t.left);
                    if (left)
                        return Sprawdz(i, t.right);
                    return true;
                }
                if (t.node == 'E')
                    return Sprawdz(i, t.left) == Sprawdz(i, t.right);
                return true;

            }
        }
        private Tree AnulujZast(int start, out int end)
        {
            Tree t = new Tree();
            t.node = P[start];
            end = start;
            t.type = SymbolType.Variable;
            SymbolType type = SprawdzKod(P[start]);
            if (type == SymbolType.Operator)
            {
                t.type = SymbolType.Operator;
                if (P[start] == 'N')
                    t.left = AnulujZast(start + 1, out end);
                else
                {
                    t.left = AnulujZast(start + 1, out end);
                    t.right = AnulujZast(end + 1, out end);
                }
            }
            return t;
        }
        private SymbolType SprawdzKod(char ch)
        {
            foreach (char ch1 in this.Operacje)
            {
                if (ch1 == ch)
                    return SymbolType.Operator;
            }
            return SymbolType.Variable;
        }
        private void GenerujZast()
        {
            char[] alternate = new char[P.Length];
            for (int i = 0; i < P.Length; i++)
            {
                if (SprawdzKod(P[i]) == SymbolType.Operator)
                {
                    alternate[i] = P[i];
                    continue;
                }
                if (Zmiany[P[i] - 'a'] != 0)
                    alternate[i] = Zmiany[P[i] - 'a'];
                else
                {
                    Zmiany[P[i] - 'a'] = (char)(Licznik + 'a');
                    alternate[i] = (char)(Licznik + 'a');
                    Licznik++;
                }
            } 
            this.P = new string(alternate);
        }

    }
}
