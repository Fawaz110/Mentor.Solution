using Core.Entities;

namespace Core.Specifications
{
    public class SocialMediaSpecification : BaseSpecification<SocialMedia>
    {
        public SocialMediaSpecification()
            :base()
        {
            AddIncludes();
        }

        public SocialMediaSpecification(string id = "", string titleSearchTerm = "", string baseUrl = "")
            : base(s => 
                    (string.IsNullOrEmpty(id) || s.Id == id)
                  && string.IsNullOrEmpty(titleSearchTerm) || s.Title.Contains(titleSearchTerm)
                  && string.IsNullOrEmpty(baseUrl) || s.BaseUrl.Contains(baseUrl))
        {
            AddIncludes(); 
        }

        private void AddIncludes()
        {
            Includes.Add(s => s.SocialMediaLinks);
        }
    }
}
