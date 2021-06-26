﻿using System;
using System.Text;
using System.Net;
using System.IO;

namespace envio_sms_altiria
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Prueba de envío de SMS con HttpWebRequest 4.0.");

            //Se fija la URL sobre la que enviar la petición POST
            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create("http://www.altiria.net/api/http");

            // Compone el mensaje a enviar
            // XX, YY y ZZ se corresponden con los valores de identificación del usuario en el sistema.
            string lcPostData = "cmd=sendsms&domainId=test&login=zutjmx@gmail.com&passwd=9bg5a94y&dest=525520950013" +
                                "&msg=Esto es un mensaje de prueba desde código C#";
            //No es posible utilizar el remitente en América pero sí en España y Europa
            //Descomentar la línea solo si se cuenta con un remitente autorizado por Altiria
            //cmd=cmd + "&senderId=remitente";

            // Se codifica en utf-8
            byte[] lbPostBuffer = System.Text.Encoding.GetEncoding("utf-8").GetBytes(lcPostData);
            loHttp.Method = "POST";
            loHttp.ContentType = "application/x-www-form-urlencoded";
            loHttp.ContentLength = lbPostBuffer.Length;

            //Fijamos tiempo de espera de respuesta = 60 seg
            loHttp.Timeout = 60000;
            String error = "";
            String response = "";
            // Envía la peticion
            try
            {
                Stream loPostData = loHttp.GetRequestStream();
                loPostData.Write(lbPostBuffer, 0, lbPostBuffer.Length);
                loPostData.Close();
                // Prepara el objeto para obtener la respuesta
                HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
                // La respuesta vendría codificada en utf-8
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader loResponseStream =
                new StreamReader(loWebResponse.GetResponseStream(), enc);
                // Conseguimos la respuesta en una cadena de texto
                response = loResponseStream.ReadToEnd();
                loWebResponse.Close();
                loResponseStream.Close();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ConnectFailure)
                    error = "Error en la conexión";
                else if (e.Status == WebExceptionStatus.Timeout)
                    error = "Error TimeOut";
                else
                    error = e.Message;
            }
            finally
            {
                if (error != "")
                    Console.WriteLine(error);
                else
                    Console.WriteLine(response);
            }

        }
    }
}
