using System.ComponentModel.DataAnnotations;

namespace NS.CustomeConfigurationProvider
{
    public class ConfigurationEntity
    {
        [Key]
        public string Key { get; set; }
        [MaxLength(1000)]
        public string Value { get; set; }
    }
}