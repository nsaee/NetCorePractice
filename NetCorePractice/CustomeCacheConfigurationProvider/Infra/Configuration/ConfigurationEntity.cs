using System.ComponentModel.DataAnnotations;

namespace NS.CustomeConfigurationProvider
{
    public class ConfigurationEntity
    {
        [Key]
        public string Key { get; set; }
        [MaxLength(4000)]
        public string Value { get; set; }
    }
}