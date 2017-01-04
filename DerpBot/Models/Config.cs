namespace DerpBot.Models
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

        private string typeField;

        private string domainField;

        private string apikeyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationImgur
    {

        private string apikeyField;

        private byte useimgurField;

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

        private string usernameField;

        private string passwordField;

        private string client_idField;

        private string secret_idField;

        private string callback_urlField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string username
        {
            get
            {
                return this.usernameField;
            }
            set
            {
                this.usernameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
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

        private string tagsField;

        private string timeframeField;

        private string methodField;

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
        public string method
        {
            get
            {
                return this.methodField;
            }
            set
            {
                this.methodField = value;
            }
        }
    }



}
