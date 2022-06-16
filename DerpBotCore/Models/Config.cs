// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable ArrangeThisQualifier
namespace DerpBotCore.Models
{




    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class configuration
    {

        private configurationDerpibooru derpibooruField;

        private configurationImgur imgurField;

        private configurationReddit redditField;

        private configurationSub[] subreddit_configurationsField;

        /// <remarks/>
        public configurationDerpibooru derpibooru
        {
            get
            {
                return this.derpibooruField;
            }
            set
            {
                this.derpibooruField = value;
            }
        }

        /// <remarks/>
        public configurationImgur imgur
        {
            get
            {
                return this.imgurField;
            }
            set
            {
                this.imgurField = value;
            }
        }

        /// <remarks/>
        public configurationReddit reddit
        {
            get
            {
                return this.redditField;
            }
            set
            {
                this.redditField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("sub", IsNullable = false)]
        public configurationSub[] subreddit_configurations
        {
            get
            {
                return this.subreddit_configurationsField;
            }
            set
            {
                this.subreddit_configurationsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationDerpibooru
    {

        private string apikeyField;

        private string domainField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string apikey
        {
            get
            {
                return this.apikeyField;
            }
            set
            {
                this.apikeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string domain
        {
            get
            {
                return this.domainField;
            }
            set
            {
                this.domainField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationImgur
    {

        private string apikeyField;

        private string secretField;

        private byte useimgurField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string secret
        {
            get
            {
                return this.secretField;
            }
            set
            {
                this.secretField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string apikey
        {
            get
            {
                return this.apikeyField;
            }
            set
            {
                this.apikeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte useimgur
        {
            get
            {
                return this.useimgurField;
            }
            set
            {
                this.useimgurField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationReddit
    {

        private string client_idField;

        private string secret_idField;

        private string callback_urlField;

        private string refresh_tokenField;

        private string access_tokenField;


        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string access_token
        {
            get
            {
                return this.access_tokenField;
            }
            set
            {
                this.access_tokenField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string refresh_token
        {
            get
            {
                return this.refresh_tokenField;
            }
            set
            {
                this.refresh_tokenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string client_id
        {
            get
            {
                return this.client_idField;
            }
            set
            {
                this.client_idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string secret_id
        {
            get
            {
                return this.secret_idField;
            }
            set
            {
                this.secret_idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string callback_url
        {
            get
            {
                return this.callback_urlField;
            }
            set
            {
                this.callback_urlField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSub
    {

        private string subredditField;

        private string timeframeField;

        private string tagsField;

        private string sensitivetagsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string subreddit
        {
            get
            {
                return this.subredditField;
            }
            set
            {
                this.subredditField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string timeframe
        {
            get
            {
                return this.timeframeField;
            }
            set
            {
                this.timeframeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags
        {
            get
            {
                return this.tagsField;
            }
            set
            {
                this.tagsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("sensitive-tags")]
        public string sensitivetags
        {
            get
            {
                return this.sensitivetagsField;
            }
            set
            {
                this.sensitivetagsField = value;
            }
        }
    }









}
