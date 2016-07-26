using System;
using System.Collections;

namespace PauloQuicoli.Comum.Strings
{
    /// <summary>
    /// Classe Fonetico
    /// </summary>
    /// <author>Marcus Vinícius Siqueira</author>
    /// <note>O código original desta classe foi traduzido da linguagem Java.</note>
    /// http://www.marvinsiq.com
    public class Fonetico
    {
        /// <summary>
        /// Fonetiza a string recebida como parametro
        /// </summary>
        /// <param name="texto">Retorna o texto fonetizado</param>
        /// <returns></returns>
        public static string Fonetiza(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            str = str.ToUpper(); //todas as letras maiusculas
            str = RemovePrep(str); //remove as preposições
            str = RemoveAcentos(str); //remove os acentos
            str = RemoveCaracteresEspeciais(str); //remove caracteres diferentes de A-Z, 0-9
            str = FonetizaTexto(str); //fonetiza o texto
            return str;
        }

        /// <summary>
        /// Remove as preposições
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String RemovePrep(String str)
        {
            if (string.IsNullOrWhiteSpace(str)) return String.Empty;

            String[] prep = new String[18];
            int i = 0;
            prep[i++] = "DEL";
            prep[i++] = "DA";
            prep[i++] = "DE";
            prep[i++] = "DI";
            prep[i++] = "DO";
            prep[i++] = "DU";
            prep[i++] = "DAS";
            prep[i++] = "DOS";
            prep[i++] = "DEU";
            prep[i++] = "DER";
            prep[i++] = "E";
            prep[i++] = "LA";
            prep[i++] = "LE";
            prep[i++] = "LES";
            prep[i++] = "LOS";
            prep[i++] = "VAN";
            prep[i++] = "VON";
            prep[i++] = "EL";

            string[] cpalavra = str.Split(' ');
            ArrayList palavra = StringArrayToArrayList(cpalavra);            
            for (i = 0; i < palavra.Count; i++)
            {
                for (int j = 0; j < prep.Length; j++)
                {
                    if (palavra[i].ToString().CompareTo(prep[j]) == 0)
                    {
                        palavra.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            return ArrayListToStr(palavra);
        }

        /// <summary>
        /// Remove os acentos
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String RemoveAcentos(String str)
        {
            str = str.Replace('É', 'E');
            str = str.Replace('Ê', 'E');
            str = str.Replace('Ë', 'E');
            str = str.Replace('É', 'E');
            str = str.Replace('Á', 'A');
            str = str.Replace('À', 'A');
            str = str.Replace('Â', 'A');
            str = str.Replace('Ã', 'A');
            str = str.Replace('Ä', 'A');
            str = str.Replace('Ç', 'C');
            str = str.Replace('Í', 'I');
            str = str.Replace('Ó', 'O');
            str = str.Replace('Ó', 'O');
            str = str.Replace('Õ', 'O');
            str = str.Replace('Ô', 'O');
            str = str.Replace('Ú', 'U');
            str = str.Replace('Ü', 'U');
            str = str.Replace('Ñ', 'N');

            return str;
        }

        /// <summary>
        /// Remove os caracteres especiais
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String RemoveCaracteresEspeciais(String str)
        {
            string retorno = "";
            for (int i = 0; i < str.Length; i++)
            {
                char caracter = char.Parse(str.Substring(i, 1));
                int ascii = Convert.ToInt32(caracter);
                if (
                    (ascii >= 48 && ascii <= 57) || 
                    (ascii >= 65 && ascii <= 90) || 
                    (ascii >= 97 && ascii <= 122) ||
                    (caracter == '&') ||
                    (caracter == '_') ||
                    (caracter == ' ')
                    )
                {
                    retorno = retorno + caracter;
                }
            }
            return retorno;                      
        }

        private static String FonetizaTexto(String str)
        {
            //Função que faz efetivamente a substituição de letras, 
            //fonetizando o texto
            //matrizes de caracteres utilizadas para manipular o texto

            char[] foncmp = new char[256];
            char[] fonwrk = new char[256];
            char[] fonaux = new char[256];
            char[] fonfon = new char[256];
            int x, k, //contadores
                desloc, //posicao atual no vetor
                endfon, //indica se eh ultimo fonema
                copfon, //indica se o fonema deve ser copiado
                copmud, newmud; //indica se o fonema eh mudo

            //Vetor utilizado para armazenar o texto:
            //cada palavra do texto e armazenada em uma posicao do vetor
            string[] ccomponent;
            ArrayList component = new ArrayList();

            int i = 0;
            int j = 0;

            str = RemoveCaracteresMultiplos(str);

            //todos os caracteres duplicados sao eliminados
            //exemplo: SS -> S, RR -> R
            ccomponent = str.Split(' ');
            for (i = 0; i < ccomponent.Length; i++)
            {
                component.Add(ccomponent[i]);
            }
            
            //o texto eh armazenado no vetor:
            //cada palavra ocupa uma posicao do vetor
            for (desloc = 0; desloc < component.Count; desloc++)
            {
                //percorre o vetor, palavra a palavra
                for (i = 0; i < 256; i++)
                {
                    fonwrk[i] = ' ';
                    fonfon[i] = ' ';//branqueia as matrizes
                }

                foncmp = component[desloc].ToString().ToCharArray();
                fonaux = foncmp;
                
                j = 0;
                if (component[desloc].ToString().Length == 1)
                {
                    // se a palavra possuir apenas 1 caracter, nao altera
                    fonwrk[0] = foncmp[0];

                    // se o caracter for "_", troca por espaco em branco
                    if (foncmp[0] == '_')
                    {
                        fonwrk[0] = ' ';
                    }
                        // se o caracter for "E", "&" ou "I", troca por "i"
                    else if ((foncmp[0] == 'E') || (foncmp[0] == '&') || (foncmp[0] == 'I'))
                    {
                        fonwrk[0] = 'i';
                    }
                }
                    // Mais de um caracter
                else
                {
                    for (i = 0; i < component[desloc].ToString().Length; i++)
                    {
                        if (foncmp[i] == '_')
                            fonfon[i] = 'Y'; // _ -> Y    

                            //& -> i
                        else if (foncmp[i] == '&')
                            fonfon[i] = 'i';

                        else if ((foncmp[i] == 'E') || (foncmp[i] == 'Y') || (foncmp[i] == 'I'))
                            fonfon[i] = 'i';     // E, Y, I -> i

                        else if ((foncmp[i] == 'O') || (foncmp[i] == 'U'))
                            fonfon[i] = 'o';     // O, U -> u

                        else if (foncmp[i] == 'A')
                            fonfon[i] = 'a';      // A -> a

                        else if (foncmp[i] == 'S')
                            fonfon[i] = 's';     // S -> s

                        else
                            fonfon[i] = foncmp[i];
                    }

                    //caracter nao eh modificado
                    endfon = 0;
                    fonaux = fonfon;

                    // palavras formadas por apenas 3 consoantes sao dispensadas do processo de fonetizacao
                    if (fonaux[3] == ' ')
                    {
                        if ((fonaux[0] == 'a') || (fonaux[0] == 'i') || (fonaux[0] == 'o'))
                            endfon = 0;
                        else if ((fonaux[1] == 'a') || (fonaux[1] == 'i') || (fonaux[1] == 'o'))
                            endfon = 0;
                        else if ((fonaux[2] == 'a') || (fonaux[2] == 'i') || (fonaux[2] == 'o'))
                            endfon = 0;
                        else
                        {
                            endfon = 1;
                            fonwrk[0] = fonaux[0];
                            fonwrk[1] = fonaux[1];
                            fonwrk[2] = fonaux[2];
                        }
                    }

                    //se a palavra nao for formada por apenas 3 consoantes...
                    if (endfon != 1)
                    { 
                        for (i = 0; i < component[desloc].ToString().Length; i++)
                        {
                            //percorre a palavra corrente, letra a letra
                            copfon = 0;
                            copmud = 0;
                            newmud = 0;
                            //zera variaveis de controle
                            switch (fonaux[i])
                            {
                                    #region A
                                case 'a':  //se o caracter for a
                                    //se a palavra termina com As, AZ, AM, ou AN,
                                    //elimina a consoante do final da palavra
                                    if ((fonaux[i + 1] == 's') || (fonaux[i + 1] == 'Z') || (fonaux[i + 1] == 'M') || (fonaux[i + 1] == 'N'))
                                        if (fonaux[i + 2] != ' ')
                                            copfon = 1;
                                        else
                                        {
                                            fonwrk[j] = 'a';
                                            fonwrk[j + 1] = ' ';
                                            j++;
                                            i++;
                                        }
                                    else copfon = 1;
                                    break;
                                    #endregion

                                    #region B
                                case 'B':  //se o caracter for B
                                    // B nao eh modificado
                                    copmud = 1;
                                    break;
                                    #endregion

                                    #region C
                                case 'C':  //se o caracter for C
                                    x = 0;
                                    if (fonaux[i + 1] == 'i')
                                        //ci vira si
                                    {
                                        fonwrk[j] = 's';
                                        j++;
                                        break;
                                    }

                                    //coes final vira cao
                                    if ((fonaux[i + 1] == 'o') && (fonaux[i + 2] == 'i') && (fonaux[i + 3] == 's') && (fonaux[i + 4] == ' '))
                                    {
                                        fonwrk[j] = 'K';
                                        fonwrk[j + 1] = 'a';
                                        fonwrk[j + 2] = 'o';
                                        i = i + 4;
                                        break;
                                    }

                                    //ct vira t
                                    if (fonaux[i + 1] == 'T')
                                        break;

                                    // c vira k
                                    if (fonaux[i + 1] != 'H')
                                    {
                                        fonwrk[j] = 'K';
                                        newmud = 1;
                                        //   ck vira k
                                        if (fonaux[i + 1] == 'K')
                                        {
                                            i++;
                                            break;
                                        }
                                        else break;
                                    }

                                    //ch vira k para chi final, chi vogal, chini final e
                                    //chiti final
                                    //chi final ou chi vogal
                                    if (fonaux[i + 1] == 'H')
                                        if (fonaux[i + 2] == 'i')
                                            if ((fonaux[i + 3] == 'a') || (fonaux[i + 3] == 'i') || (fonaux[i + 3] == 'o'))
                                                x = 1;
                                                // chini final
                                            else if (fonaux[i + 3] == 'N')
                                                if (fonaux[i + 4] == 'i')
                                                    if (fonaux[i + 5] == ' ')
                                                        x = 1;
                                                    else { }
                                                else { }
                                            else
                                                // chiti final
                                                if (fonaux[i + 3] == 'T')
                                                    if (fonaux[i + 4] == 'i')
                                                        if (fonaux[i + 5] == ' ')
                                                            x = 1;
                                    if (x == 1)
                                    {
                                        fonwrk[j] = 'K';
                                        j++;
                                        i++;
                                        break;
                                    }

                                    //chi, nao chi final, chi vogal, chini final ou chiti final
                                    //ch nao seguido de i
                                    
                                    //se anterior nao e s, ch = x
                                    if (j > 0)
                                        //sch: fonema recua uma posicao
                                        if (fonwrk[j - 1] == 's')
                                        {
                                            j--;
                                        }
                                    fonwrk[j] = 'X';
                                    newmud = 1;
                                    i++;
                                    break;
                                    #endregion

                                    #region D
                                case 'D':  //se o caracter for D
                                    x = 0;
                                    //procura por dor
                                    if (fonaux[i + 1] != 'o')
                                    {
                                        copmud = 1;
                                        break;
                                    }
                                    else
                                        if (fonaux[i + 2] == 'R')
                                            if (i != 0)
                                                x = 1; // dor nao inicial
                                            else copfon = 1; // dor inicial
                                        else copfon = 1;  // nao e dor
                                    if (x == 1)
                                        if (fonaux[i + 3] == 'i')
                                            if (fonaux[i + 4] == 's') // dores
                                                if (fonaux[i + 5] != ' ')
                                                    x = 0;  // nao e dores
                                                else { }
                                            else x = 0;
                                        else
                                            if (fonaux[i + 3] == 'a')
                                                if (fonaux[i + 4] != ' ')
                                                    if (fonaux[i + 4] != 's')
                                                        x = 0;
                                                    else
                                                        if (fonaux[i + 5] != ' ')
                                                            x = 0;
                                                        else { }
                                                else { }
                                            else x = 0;
                                    else x = 0;

                                    if (x == 1)
                                    {
                                        fonwrk[j] = 'D';
                                        fonwrk[j + 1] = 'o';
                                        fonwrk[j + 2] = 'R';
                                        i = i + 5;
                                    }
                                    else copfon = 1;

                                    break;
                                    #endregion

                                    #region F
                                case 'F':  //se o caracter for F
                                    //F nao eh modificado
                                    copmud = 1;
                                    break;
                                    #endregion

                                    #region G
                                case 'G':  //se o caracter for G
                                    //gui -> gi
                                    if (fonaux[i + 1] == 'o')
                                    {
                                        if (fonaux[i + 2] == 'i')
                                        {
                                            fonwrk[j] = 'G';
                                            fonwrk[j + 1] = 'i';
                                            j += 2;
                                            i += 2;
                                        }
                                            //diferente de gui copia como consoante muda
                                        else copmud = 1;
                                    }
                                    else
                                        //gl
                                        if (fonaux[i + 1] == 'L')
                                            if (fonaux[i + 2] == 'i')
                                                //gli + vogal -> li + vogal
                                                if ((fonaux[i + 3] == 'a') ||
                                                    (fonaux[i + 3] == 'i') ||
                                                    (fonaux[i + 3] == 'o'))
                                                {
                                                    fonwrk[j] = fonaux[i + 1];
                                                    fonwrk[j + 1] = fonaux[i + 2];
                                                    j += 2;
                                                    i += 2;
                                                }
                                                else
                                                    //glin -> lin
                                                    if (fonaux[i + 3] == 'N')
                                                    {
                                                        fonwrk[j] = fonaux[i + 1];
                                                        fonwrk[j + 1] = fonaux[i + 2];
                                                        j += 2;
                                                        i += 2;

                                                    }/*if*/
                                                    else copmud = 1;
                                            else copmud = 1;
                                        else
                                            //gn + vogal -> ni + vogal
                                            if (fonaux[i + 1] == 'N')
                                                if ((fonaux[i + 2] != 'a') &&
                                                    (fonaux[i + 2] != 'i') &&
                                                    (fonaux[i + 2] != 'o'))
                                                    copmud = 1;
                                                else
                                                {
                                                    fonwrk[j] = 'N';
                                                    fonwrk[j + 1] = 'i';
                                                    j += 2;
                                                    i++;
                                                }

                                            else
                                                //   ghi -> gi
                                                if (fonaux[i + 1] == 'H')
                                                    if (fonaux[i + 2] == 'i')
                                                    {
                                                        fonwrk[j] = 'G';
                                                        fonwrk[j + 1] = 'i';
                                                        j += 2;
                                                        i += 2;
                                                    }
                                                    else copmud = 1;
                                                else copmud = 1;
                                    break;
                                    #endregion

                                    #region H
                                case 'H':  //se o caracter for H
                                    //H eh desconsiderado
                                    break;
                                    #endregion

                                    #region I
                                case 'i':  //se o caracter for i
                                    if (fonaux[i + 2] == ' ')
                                        //is ou iz final perde a consoante
                                        if (fonaux[i + 1] == 's')
                                        {
                                            fonwrk[j] = 'i';
                                            break;
                                        }
                                        else
                                            if (fonaux[i + 1] == 'Z')
                                            {
                                                fonwrk[j] = 'i';
                                                break;
                                            }
                                    //ix
                                    if (fonaux[i + 1] != 'X')
                                        copfon = 1;
                                    else
                                        if (i != 0)
                                            copfon = 1;
                                        else
                                            //ix vogal no inicio torna-se iz
                                            if ((fonaux[i + 2] == 'a') ||
                                                (fonaux[i + 2] == 'i') ||
                                                (fonaux[i + 2] == 'o'))
                                            {
                                                fonwrk[j] = 'i';
                                                fonwrk[j + 1] = 'Z';
                                                j += 2;
                                                i++;
                                                break;
                                            }
                                            else
                                                //ix consoante no inicio torna-se is
                                                if (fonaux[i + 2] == 'C' || fonaux[i + 2] == 's')
                                                {
                                                    fonwrk[j] = 'i';
                                                    j++;
                                                    i++;
                                                    break;
                                                }
                                                else
                                                {
                                                    fonwrk[j] = 'i';
                                                    fonwrk[j + 1] = 's';
                                                    j += 2;
                                                    i++;
                                                    break;
                                                }
                                    break;
                                    #endregion

                                    #region J
                                case 'J':  //se o caracter for J
                                    //J -> Gi
                                    fonwrk[j] = 'G';
                                    fonwrk[j + 1] = 'i';
                                    j += 2;
                                    break;
                                    #endregion

                                    #region K
                                case 'K':  //se o caracter for K
                                    //KT -> T
                                    if (fonaux[i + 1] != 'T')
                                        copmud = 1;
                                    break;
                                    #endregion

                                    #region L
                                case 'L':  //se o caracter for L
                                    //L + vogal nao eh modificado
                                    if ((fonaux[i + 1] == 'a') ||
                                        (fonaux[i + 1] == 'i') ||
                                        (fonaux[i + 1] == 'o'))
                                        copfon = 1;
                                    else
                                        //L + consoante -> U + consoante
                                        if (fonaux[i + 1] != 'H')
                                        {
                                            fonwrk[j] = 'o';
                                            j++;
                                            break;
                                        }
                                            //LH + consoante nao eh modificado
                                        else
                                            if (fonaux[i + 2] != 'a' &&
                                                fonaux[i + 2] != 'i' &&
                                                fonaux[i + 2] != 'o')
                                                copfon = 1;
                                            else
                                                //LH + vogal -> LI + vogal
                                            {
                                                fonwrk[j] = 'L';
                                                fonwrk[j + 1] = 'i';
                                                j += 2;
                                                i++;
                                                break;
                                            }
                                    break;
                                    #endregion

                                    #region M
                                case 'M':  //se o caracter for M
                                    //M + consoante -> N + consoante
                                    //M final -> N
                                    if ((fonaux[i + 1] != 'a' &&
                                         fonaux[i + 1] != 'i' &&
                                         fonaux[i + 1] != 'o') ||
                                        (fonaux[i + 1] == ' '))
                                    {
                                        fonwrk[j] = 'N';
                                        j++;
                                    }
                                        //M nao eh alterado
                                    else copfon = 1;
                                    break;
                                    #endregion

                                    #region N
                                case 'N':  //se o caracter for N
                                    //NGT -> NT
                                    if ((fonaux[i + 1] == 'G') &&
                                        (fonaux[i + 2] == 'T'))
                                    {
                                        fonaux[i + 1] = 'N';
                                        copfon = 1;
                                    }
                                    else
                                        //NH + consoante nao eh modificado
                                        if (fonaux[i + 1] == 'H')
                                            if ((fonaux[i + 2] != 'a') &&
                                                (fonaux[i + 2] != 'i') &&
                                                (fonaux[i + 2] != 'o'))
                                                copfon = 1;
                                                //NH + vogal -> Ni + vogal
                                            else
                                            {
                                                fonwrk[j] = 'N';
                                                fonwrk[j + 1] = 'i';
                                                j += 2;
                                                i++;
                                            }
                                        else copfon = 1;
                                    break;
                                    #endregion

                                    #region O
                                case 'o':  //se o caracter for o
                                    //oS final -> o
                                    //oZ final -> o
                                    if ((fonaux[i + 1] == 's') ||
                                        (fonaux[i + 1] == 'Z'))
                                        if (fonaux[i + 2] == ' ')
                                        {
                                            fonwrk[j] = 'o';
                                            break;
                                        }
                                        else copfon = 1;
                                    else copfon = 1;
                                    break;
                                    #endregion

                                    #region P
                                case 'P':  //se o caracter for P
                                    //PH -> F
                                    if (fonaux[i + 1] == 'H')
                                    {
                                        fonwrk[j] = 'F';
                                        i++;
                                        newmud = 1;
                                    }
                                    else
                                        copmud = 1;
                                    break;
                                    #endregion

                                    #region Q
                                case 'Q':  //se o caracter for Q
                                    //Koi -> Ki (QUE, QUI -> KE, KI)
                                    if (fonaux[i + 1] == 'o')
                                        if (fonaux[i + 2] == 'i')
                                        {
                                            fonwrk[j] = 'K';
                                            j++;
                                            i++;
                                            break;
                                        }
                                    //QoA -> KoA (QUA -> KUA)
                                    fonwrk[j] = 'K';
                                    j++;
                                    break;
                                    #endregion

                                    #region R
                                case 'R':  //se o caracter for R
                                    //R nao eh modificado
                                    copfon = 1;
                                    break;
                                    #endregion

                                    #region S
                                case 's':  //se o caracter for s
                                    //s final eh ignorado
                                    if (fonaux[i + 1] == ' ')
                                        break;

                                    //s inicial + vogal nao eh modificado
                                    if ((fonaux[i + 1] == 'a') ||
                                        (fonaux[i + 1] == 'i') ||
                                        (fonaux[i + 1] == 'o'))
                                        if (i == 0)
                                        {
                                            copfon = 1;
                                            break;
                                        }
                                        else
                                            //s entre duas vogais -> z
                                            if ((fonaux[i - 1] != 'a') &&
                                                (fonaux[i - 1] != 'i') &&
                                                (fonaux[i - 1] != 'o'))
                                            {
                                                copfon = 1;
                                                break;
                                            }
                                            else
                                                //SoL nao eh modificado
                                                if ((fonaux[i + 1] == 'o') &&
                                                    (fonaux[i + 2] == 'L') &&
                                                    (fonaux[i + 3] == ' '))
                                                {
                                                    copfon = 1;
                                                    break;
                                                }
                                                else
                                                {
                                                    fonwrk[j] = 'Z';
                                                    j++;
                                                    break;
                                                }
                                    //ss -> s
                                    if (fonaux[i + 1] == 's')
                                        if (fonaux[i + 2] != ' ')
                                        {
                                            copfon = 1;
                                            i++;
                                            break;
                                        }
                                        else
                                        {
                                            fonaux[i + 1] = ' ';
                                            break;
                                        }
                                    //s inicial seguido de consoante fica precedido de i
                                    //se nao for sci, sh ou sch nao seguido de vogal
                                    if (i == 0)
                                        if (!((fonaux[i + 1] == 'C') &&
                                              (fonaux[i + 2] == 'i')))
                                            if (fonaux[i + 1] != 'H')
                                                if (!((fonaux[i + 1] == 'C') &&
                                                      (fonaux[i + 2] == 'H') &&
                                                      ((fonaux[i + 3] != 'a') &&
                                                       (fonaux[i + 3] != 'i') &&
                                                       (fonaux[i + 3] != 'o'))))
                                                {
                                                    fonwrk[j] = 'i';
                                                    j++;
                                                    copfon = 1;
                                                    break;
                                                }
                                    //sH -> X;
                                    if (fonaux[i + 1] == 'H')
                                    {
                                        fonwrk[j] = 'X';
                                        i++;
                                        newmud = 1;
                                        break;
                                    }
                                    if (fonaux[i + 1] != 'C')
                                    {
                                        copfon = 1;
                                        break;
                                    }
                                    //   sCh nao seguido de i torna-se X
                                    if (fonaux[i + 2] == 'H')
                                    {
                                        fonwrk[j] = 'X';
                                        i += 2;
                                        newmud = 1;
                                        break;
                                    }
                                    if (fonaux[i + 2] != 'i')
                                    {
                                        copfon = 1;
                                        break;
                                    }

                                    //sCi final -> Xi
                                    if (fonaux[i + 3] == ' ')
                                    {
                                        fonwrk[j] = 'X';
                                        fonwrk[j + 1] = 'i';
                                        i = i + 3;
                                        break;
                                    }

                                    //sCi vogal -> X
                                    if ((fonaux[i + 3] == 'a') ||
                                        (fonaux[i + 3] == 'i') ||
                                        (fonaux[i + 3] == 'o'))
                                    {
                                        fonwrk[j] = 'X';
                                        j++;
                                        i += 2;
                                        break;
                                    }

                                    //sCi consoante -> si
                                    fonwrk[j] = 's';
                                    fonwrk[j + 1] = 'i';
                                    j += 2;
                                    i += 2;
                                    break;
                                    #endregion

                                    #region T
                                case 'T':  //se o caracter for T
                                    //TS -> S
                                    if (fonaux[i + 1] == 's')
                                        break;

                                        //TZ -> Z
                                    else
                                        if (fonaux[i + 1] == 'Z')
                                            break;
                                        else copmud = 1;
                                    break;
                                    #endregion

                                    #region V
                                case 'V':  //se o caracter for V
                                    #endregion

                                    #region W
                                case 'W':  //ou se o caracter for W
                                    //V,W inicial + vogal -> o + vogal (U + vogal)
                                    if (fonaux[i + 1] == 'a' ||
                                        fonaux[i + 1] == 'i' ||
                                        fonaux[i + 1] == 'o')
                                        if (i == 0)
                                        {
                                            fonwrk[j] = 'o';
                                            j++;
                                        }

                                            //V,W NAO inicial + vogal -> V + vogal
                                        else
                                        {
                                            fonwrk[j] = 'V';
                                            newmud = 1;
                                        }

                                    else
                                    {
                                        fonwrk[j] = 'V';
                                        newmud = 1;
                                    }
                                    break;
                                    #endregion

                                    #region X
                                case 'X':  //se o caracter for X
                                    //caracter nao eh modificado
                                    copmud = 1;
                                    break;
                                    #endregion

                                    #region Y
                                case 'Y':  //se o caracter for Y
                                    //Y jah foi tratado acima
                                    break;
                                    #endregion

                                    #region Z
                                case 'Z':  //se o caracter for Z
                                    //Z final eh eliminado
                                    if (fonaux[i + 1] == ' ')
                                        break;
                                        //Z + vogal nao eh modificado
                                    else
                                        if ((fonaux[i + 1] == 'a') ||
                                            (fonaux[i + 1] == 'i') ||
                                            (fonaux[i + 1] == 'o'))
                                            copfon = 1;
                                            //Z + consoante -> S + consoante
                                        else
                                        {
                                            fonwrk[j] = 's';
                                            j++;
                                        }

                                    break;
                                    #endregion

                                default: //se o caracter nao for um dos jah relacionados
                                    //o caracter nao eh modificado
                                    fonwrk[j] = fonaux[i];
                                    j++;
                                    break;
                            }//switch

                            //copia caracter corrente
                            if (copfon == 1)
                            {
                                fonwrk[j] = fonaux[i];
                                j++;
                            }

                            //insercao de i apos consoante muda
                            if (copmud == 1)
                                fonwrk[j] = fonaux[i];

                            if (copmud == 1 || newmud == 1)
                            {
                                j++;
                                k = 0;
                                while (k == 0)
                                    if (fonaux[i + 1] == ' ')
                                        //e final mudo
                                    {
                                        fonwrk[j] = 'i';
                                        k = 1;
                                    }

                                    else
                                        if ((fonaux[i + 1] == 'a') ||
                                            (fonaux[i + 1] == 'i') ||
                                            (fonaux[i + 1] == 'o'))
                                            k = 1;
                                        else
                                            if (fonwrk[j - 1] == 'X')
                                            {
                                                fonwrk[j] = 'i';
                                                j++;
                                                k = 1;
                                            }
                                            else
                                                if (fonaux[i + 1] == 'R')
                                                    k = 1;
                                                else
                                                    if (fonaux[i + 1] == 'L')
                                                        k = 1;
                                                    else
                                                        if (fonaux[i + 1] != 'H')
                                                        {
                                                            fonwrk[j] = 'i';
                                                            j++;
                                                            k = 1;
                                                        }
                                                        else i++;
                            }
                        }//for
                    }
                }

                for (i = 0; i < component[desloc].ToString().Length + 3; i++)
                    //percorre toda a palavra, letra a letra
                    //i -> I
                    if (fonwrk[i] == 'i')
                        fonwrk[i] = 'I';
                    else
                        //a -> A
                        if (fonwrk[i] == 'a')
                            fonwrk[i] = 'A';
                        else
                            //o -> U
                            if (fonwrk[i] == 'o')
                                fonwrk[i] = 'U';
                            else
                                //s -> S
                                if (fonwrk[i] == 's')
                                    fonwrk[i] = 'S';
                                else
                                    //E -> b
                                    if (fonwrk[i] == 'E')
                                        fonwrk[i] = ' ';
                                    else
                                        //Y -> _
                                        if (fonwrk[i] == 'Y')
                                            fonwrk[i] = '_';

                //retorna a palavra, modificada, ao vetor que contem o texto                    
                string strfonwrk = "";
                int l = 0;
                while (fonwrk[l] != ' ')
                {
                    strfonwrk = strfonwrk + fonwrk[l];
                    l++;
                }
                component[desloc] = strfonwrk;
                j = 0; //zera o contador
            }//for
            str = ArrayListToStr(component);

            //remonta as palavras armazenadas no vetor em um unico string
            str = RemoveCaracteresMultiplos(str);

            //remove os caracteres duplicados
            return str.ToUpper().Trim();
        }

        /// <summary>
        /// Remove os carateres que estao multiplicados. Ex.:ss, rr
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static String RemoveCaracteresMultiplos(String str)
        {
            String foncmp = "";
            
            //matriz de caracteres que armazena o texto sem duplicatas
            char[] fonaux = new char[256];
            
            //matriz de caracteres que armazena o texto original
            char[] tip = new char[1]; //armazena o caracter anterior
            
            tip[0] = ' ';
            fonaux = str.ToCharArray();
            
            for (int i = 0; i < str.Length; i++)
            {
                //percorre o texto, caracter a caracter.
                //elimina o caracter se ele for duplicata e se nao for numero, espaco ou S
                if ((fonaux[i] != tip[0]) || (fonaux[i] == ' ')
                    || ((fonaux[i] >= '0') && (fonaux[i] <= '9'))
                    || ((fonaux[i] == 'S') && (fonaux[i - 1] == 'S') &&
                        ((i > 1) && (fonaux[i - 2] != 'S'))))
                {
                    foncmp += fonaux[i];
                }
                tip[0] = fonaux[i];
                //reajusta o caracter de comparacao
            }
            return foncmp.Trim();
        }

        /// <summary>
        /// Converte um vetor de strings em um ArrayList
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ArrayList StringArrayToArrayList(string[] str)
        {
            ArrayList array = new ArrayList();
            for (int i = 0; i < str.Length; i++)
            {
                array.Add(str[i]);
            }
            return array;
        }

        /// <summary>
        /// Converte o texto em um ArrayList onde cada palavra do texto ocupa uma posicao do Array
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ArrayList strToArrayList(String str)
        {           
            str = str.Trim();

            char[] fonaux = new char[256];
            char[] foncmp = new char[256];

            ArrayList component = new ArrayList();

            int first = 1; //posicao da matriz
            int pos = 0; //indica se eh espaco em branco repetido
            int rep = 0; //indica se eh o primeiro caracter

            fonaux = str.ToCharArray();

            for (int j = 0; j < 256; j++)
                foncmp[j] = ' ';

            for (int i = 0; i < str.Length; i++)
            {
                //percorre o texto, caracter a caracter
                //se encontrar um espaco e nao for o primeiro caracter,
                //armazena a palavra no vetor
                if ((fonaux[i] == ' ') && (first != 1))
                {
                    if (rep == 0)
                    {
                        component.Add(foncmp.ToString().Trim());

                        pos = 0;
                        rep = 1;

                        for (int j = 0; j < 256; j++)
                            foncmp[j] = ' ';
                    }
                }
                else
                {
                    foncmp[pos] = fonaux[i];
                    first = 0;
                    pos++;
                    rep = 0;
                }
            }
            
            if (foncmp[0] != ' ')
                component.Add(foncmp.ToString().Trim());

            return component;
        }
        
        /// <summary>
        /// Converte um ArrayList em uma string onde cada posição do vetor é separada por espaços em branco.
        /// </summary>
        /// <param name="vtr"></param>
        /// <returns></returns>
        public static String ArrayListToStr(ArrayList vtr)
        {
            string str = "";
            for (int i = 0; i < vtr.Count; i++)
            {
                str += vtr[i] + " ";
            }            
            return str.Trim();
        }
    }
}