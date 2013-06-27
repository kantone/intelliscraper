using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IntelliScraper.Scrape
{
    public static class PostProcessUtils
    {
     
        public static string startEndWith(this string val, Db.startEndWith a)
        {
            //start with
            if (a.type == Db.startEndWithType.both || a.type == Db.startEndWithType.startWith)
            {
                if (a.startEndWithEnabled && !string.IsNullOrEmpty(a.starEndtWithVal) && val.StartsWith(a.starEndtWithVal))
                {
                    Factory.Instance.iInfo(string.Format("If start with append before e/o after"));
                    if (a.attachType == Db.startEndWithAttachType.before)
                        val = a.startEndWithAddVal + val;
                    if (a.attachType == Db.startEndWithAttachType.after)
                        val = val + a.startEndWithAddVal;
                    if (a.attachType == Db.startEndWithAttachType.both)
                        val = a.startEndWithAddVal + val + a.startEndWithAddVal;
                }

            }

            //end with
            if (a.type == Db.startEndWithType.both || a.type == Db.startEndWithType.endWith)
            {

                if (a.startEndWithEnabled && !string.IsNullOrEmpty(a.starEndtWithVal) && val.EndsWith(a.starEndtWithVal))
                {
                    Factory.Instance.iInfo(string.Format("If end with append before e/o after"));
                    if (a.attachType == Db.startEndWithAttachType.before)
                        val = a.startEndWithAddVal + val;
                    if (a.attachType == Db.startEndWithAttachType.after)
                        val = val + a.startEndWithAddVal;
                    if (a.attachType == Db.startEndWithAttachType.both)
                        val = a.startEndWithAddVal + val + a.startEndWithAddVal;
                }


            }
            return val;
        }

        public static string startEndWith(this string val, string findId)
        {
            Db.startEndWith t = (from x in Factory.Instance.i.postProcess.startEndWith where x.id == findId select x).FirstOrDefault();
            if (t != null)
                return val.startEndWith(t);
            return val;
        }

        public static string htmlDecodeEncode(this string val, Db.htmlEncodeDecode a)
        {
            //Decode
            if (a.HtmlDecode)
            {
                Factory.Instance.iInfo(string.Format("HtmlDecode  {0}", val));
                val = System.Web.HttpUtility.HtmlDecode(val);
            }

            //Encode
            if (a.HtmlEncode)
            {
                Factory.Instance.iInfo(string.Format("HtmlEncode  {0}", val));
                val = System.Web.HttpUtility.HtmlEncode(val);
            }
            return val;
        }

        public static string htmlDecodeEncode(this string val, string findId)
        {
            Db.htmlEncodeDecode t = (from x in Factory.Instance.i.postProcess.htmlEncodeDecode where x.id == findId select x).FirstOrDefault();
            if (t != null)
                return val.htmlDecodeEncode(t);
            return val;
        }

        public static string substring(this string txt, Db.substring a)
        {
            Factory.Instance.log.Info(string.Format("Make Substring of {0}", txt));
            if (a.type ==  Db.substringType.simple && a.substringFrom > 0)
            {
                Factory.Instance.iInfo(string.Format("Substring from {0}", a.substringFrom));
                txt = txt.Substring(a.substringFrom);
            }
            if (a.type == Db.substringType.fromTo && a.substringFrom > 0 && a.substringTo > 0)
            {
                Factory.Instance.iInfo(string.Format("Substring from {0} to {1}", a.substringFrom, a.substringTo));
                txt = txt.Substring(a.substringFrom, a.substringTo);
            }

            if (a.type == Db.substringType.search && !string.IsNullOrEmpty(a.substringSearch))
            {
                Factory.Instance.iInfo(string.Format("Substring from - by string search - {0}", a.substringSearch));
                int index = txt.IndexOf(a.substringSearch);
                if (index > 0)
                {
                    txt = txt.Substring(index + 1);
                }
            }

            if (a.type == Db.substringType.searchTo && !string.IsNullOrEmpty(a.substringSearch))
            {
                Factory.Instance.iInfo(string.Format("Substring from/to - by string search - {0}", a.substringSearch));
                int index = txt.IndexOf(a.substringSearch);
                if (index > 0)
                {
                    txt = txt.Substring(0, index);
                }
            }
            return txt;
        }

        public static string substring(this string val, string findId)
        {
            Db.substring t = (from x in Factory.Instance.i.postProcess.substring where x.id == findId select x).FirstOrDefault();
            if (t != null)
                return val.substring(t);
            return val;
        }

        public static string trim(this string val, Db.trim a)
        {

            Factory.Instance.iInfo(string.Format("Make Trim of {0}", val));
            if (a.type == Db.trimType.both)
                val = val.Trim(a.trimValue.ToCharArray());

            if (a.type == Db.trimType.trimStart)
                val = val.TrimStart(a.trimValue.ToCharArray());

            if (a.type == Db.trimType.trimEnd)
                val = val.TrimEnd(a.trimValue.ToCharArray());

            return val;
        }

        public static string trim(this string val, string findId)
        {
            Db.trim t = (from x in Factory.Instance.i.postProcess.trim where x.id == findId select x).FirstOrDefault();
            if (t != null)
                return val.trim(t);
            return val;
        }
        
        public static string replace(this string val, Db.replace a)
        {
            Factory.Instance.iInfo(string.Format("Replace text {0} with {1}", a.findText, a.replaceText));
            if (!string.IsNullOrEmpty(a.findText) && !string.IsNullOrEmpty(a.replaceText))
            {
                val = val.Replace(a.findText, a.replaceText);
            }
            return val;
        }

        public static string replace(this string val, string findId)
        {
            Db.replace t = (from x in Factory.Instance.i.postProcess.replace where x.id == findId select x).FirstOrDefault();
            if (t != null)
                return val.replace(t);
            return val;
        }
        
        public static string regularExpression(this string val, Db.regularExpression a)
        {
            //regular expression
            if (a.regularExpressionValue != null)
            {
                foreach (string expr in a.regularExpressionValue)
                {
                    if (!string.IsNullOrEmpty(expr))
                    {
                        Match match = Regex.Match(val, expr);
                        val = match.Value;
                    }
                }
            }
            return val;
        }

        public static string regularExpression(this string val, string findId)
        {
            Db.regularExpression t = (from x in Factory.Instance.i.postProcess.regularExpression where x.id == findId select x).FirstOrDefault();
            if (t != null)
                return val.regularExpression(t);
            return val;
        }
        
        public static string append(this string val, Db.append a)
        {

            if (a.type == Db.appendType.after || a.type == Db.appendType.both)
                val = val + a.appendValue;

            if (a.type == Db.appendType.before || a.type == Db.appendType.both)
                val = a.appendValue + val;
            return val;
        }
        
        public static string append(this string val, string findId)
        {
            Db.append t = (from x in Factory.Instance.i.postProcess.append where x.id == findId select x).FirstOrDefault();
            if (t != null)            
                return val.append(t);            
            return val;
        }
    }
}
