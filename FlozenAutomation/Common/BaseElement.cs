namespace FlozenAutomation.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenQA.Selenium;

    public class BaseElement
    {
        public string ElementType { get; set; }
        public string ElementLocator { get; set; }

        public BaseElement(string elementType, string elementLocator)
        {
            this.ElementType = elementType;
            this.ElementLocator = elementLocator;
        }

        public BaseElement ReplaceToken(string tokenName, string tokenValue)
        {
            this.ElementLocator = this.ElementLocator.Replace(tokenName, tokenValue);
            return this;
        }

        public By ToBy()
        {
            By by = null;

            switch (this.ElementType.ToLower())
            {
                case "id":
                    by = By.Id(ElementLocator);
                    break;
                case "name":
                    by = By.Name(ElementLocator);
                    break;
                case "class":
                    by = By.ClassName(ElementLocator);
                    break;
                case "xpath":
                    by = By.XPath(ElementLocator);
                    break;
                case "linktext":
                    by = By.LinkText(ElementLocator);
                    break;
                case "partiallinktext":
                    by = By.PartialLinkText(ElementLocator);
                    break;
                case "css":
                    by = By.CssSelector(ElementLocator);
                    break;
                default:
                    by = By.Id(ElementLocator);
                    break;
            }

            return by;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.ElementType + "-" + this.ElementLocator);

            return sb.ToString();
        }
    }
}
