﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebToaster
{
    static class RequestHandler
    {
        class CallInfo
        {
            string _message;

            public CallInfo(NameValueCollection p)
            {
                CallerID = PhoneNumber = string.Empty;

                if (p.AllKeys.Contains("caller"))
                {
                    CallerID = p["caller"];
                }
                if (p.AllKeys.Contains("number"))
                {
                    PhoneNumber = p["number"];
                }
                if (p.AllKeys.Contains("msg"))
                {
                    _message = p["msg"];
                }
            }

            public string PhoneNumber;
            public string CallerID;

            public string Message
            {
                get
                { 
                    if (string.IsNullOrEmpty(CallerID) && string.IsNullOrEmpty(PhoneNumber))
                    {
                        return _message;
                    }
                    return string.Format("Incoming Call: {0} ({1})", CallerID, PhoneNumber); 
                }
            }
        }

        public static void ListenerCallback(IAsyncResult result)
        {
            HttpListener l = (HttpListener)result.AsyncState;

            if (l.IsListening)
            {
                var context = l.EndGetContext(result);
                Notifier n = new Notifier();
                CallInfo c = new CallInfo(context.Request.QueryString);
                n.PopUp(c.Message);
                context.Response.Close();
            }
        }
    }
}
