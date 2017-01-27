using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Web.Security;

using EPS.Entities;
using EPS.BusinessLayer;
using EPS.Resources;
using EPS.Utilities;

public partial class Admin_MasterData : System.Web.UI.Page
{
    public static string Client = "";
    public static string ProjectName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
        Client = profile.Client;
        ProjectName = profile.Project;
    }

    [WebMethod(EnableSession = true)]
    public static string IsUserAuthorized(string userName)
    {
        //if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) || HttpContext.Current.User.Identity.Name.ToLower() != userName.ToLower())
        //    return JsonConvert.SerializeObject(false).ToString();

        ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(userName);
        if (profile != null && !profile.IsAdmin)
            return JsonConvert.SerializeObject(false).ToString();

        MembershipUser User = Membership.GetUser(userName, true);
        if (User.LastPasswordChangedDate < DateTime.Now.AddDays(EPS.Utilities.ConfigurationHelper.PasswordExpiryPeriod))
            return JsonConvert.SerializeObject("ChangePassword");

        return JsonConvert.SerializeObject(true).ToString();
    }


    [WebMethod(EnableSession = true)]
    public static string GetLookupDataCategories()
    {
        try
        {
            using (var lookupBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                var lookUpList = lookupBLObject.GetLookupCategories(profile.ClientProjectId);

                var json = JsonConvert.SerializeObject(lookUpList).ToString();
                return json;
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    //GetCommentsCategories

    [WebMethod(EnableSession = true)]
    public static string GetCommentsCategories()
    {
        try
        {
            using (var CommentsCategoriesBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                var commentsCategoriesList = CommentsCategoriesBLObject.GetCommentsCategories(profile.ClientProjectId).Select(l => new List<string> 
                    {
                        l.Id.ToString(),
                        l.Name.ToString()   
                    }).ToList();

                var json = JsonConvert.SerializeObject(commentsCategoriesList).ToString();
                return json;
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string GetLookupDatum(string category)
    {
        try
        {
            using (var lookupDatumBLObject = new EPS.BusinessLayer.Lookup())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
                var lookUpDatum = lookupDatumBLObject.GetLookupByCategory(profile.ClientProjectId, category);

                var lookupData = (from l in lookUpDatum
                                  select new List<string>
                                  {
                                      l.Id.ToString(),
                                      l.ClientProject.Client,
                                      l.Name,
                                      l.Category,
                                      l.DisplayOrder.ToString(),
                                      l.IsActive.ToString()
                                  }).ToList();

                var json = JsonConvert.SerializeObject(lookupData).ToString();
                return json;
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string UpdateLookupData(int lookupId, string lookupName, int lookupDisplayOrder, string lookupCategory, int isActive)
    {
        try
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

            using (var lookupBL = new EPS.BusinessLayer.Lookup())
            {
                var lookupObject = new EPS.Entities.Lookup()
                {
                    Id = lookupId,
                    Name = lookupName,
                    ClientProjectId = profile.ClientProjectId,
                    Category = lookupCategory,
                    DisplayOrder = lookupDisplayOrder,
                    IsActive = Convert.ToBoolean(isActive)
                };

                return lookupBL.InsertUpdateLookup(lookupObject);
            }
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }
    }

    [WebMethod(EnableSession = true)]
    public static string InsertLookupData(string lookupName, int lookupDisplayOrder, string lookupCategory, int isActive)
    {
        try
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
            var clientProjectId = (lookupCategory.ToLower() == "location" || lookupCategory.ToLower() == "l3leveldisagree") ? new Nullable<int>() : profile.ClientProjectId;
            using (var lookupBL = new EPS.BusinessLayer.Lookup())
            {
                if (lookupBL.ValidateLookupByCategory(clientProjectId, lookupCategory, lookupName, 0))
                {
                    var lookupObject = new EPS.Entities.Lookup()
                    {
                        Id = 0,
                        Name = lookupName,
                        ClientProjectId = clientProjectId,
                        Category = lookupCategory,
                        DisplayOrder = lookupDisplayOrder,
                        IsActive = Convert.ToBoolean(isActive)
                    };

                    lookupBL.InsertUpdateLookup(lookupObject);
                }
                else
                    return "[\" 2 \"]";
            }
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return "[\" 1 \"]";
    }

    ///////////////////////////////// TAB 2 

    [WebMethod(EnableSession = true)]
    public static string GetCommentsByCategoryTable(int commentCategoryId)
    {
        try
        {
            using (var commentsBLObject = new EPS.BusinessLayer.Comments())
            {
                var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

                var comments = commentsBLObject.GetCommentsByCategory(profile.ClientProjectId, commentCategoryId, null)
                    .Select(p => new List<string>{
                        p.Id.ToString(),
                        p.Description,
                        p.DisplayOrder.ToString(),
                        p.isActive.ToString()
                    }).ToList();

                var json = JsonConvert.SerializeObject(comments).ToString();
                return json;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    [WebMethod(EnableSession = true)]
    public static string InsertUpdateComments(int commentId, string commentDescription, int commentCategoryId, int displayOrder, bool commentIsActive)
    {
        try
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

            using (var commentCategoryBL = new EPS.BusinessLayer.Comments())
            {
                var commentCategoryObject = new EPS.Entities.Comments()
                   {
                       Id = commentId,
                       Description = commentDescription,
                       ClientProjectId = profile.ClientProjectId,
                       CommentCategoryId = commentCategoryId,
                       DisplayOrder = displayOrder,
                       isActive = commentIsActive
                   };

                commentCategoryBL.InsertUpdateComments(commentCategoryObject);
            }
        }
        catch (ApplicationException)
        {
            return "[\" 2 \"]";
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return "[\" 1 \"]";
    }

    [WebMethod(EnableSession = true)]
    public static string GetQueue()
    {
        var profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);
        DataSet objDS;

        try
        {
            using (EPS.DataLayer.SiteMaster objSM = new EPS.DataLayer.SiteMaster())
            {
                objDS = objSM.getQueueCategory(profile.ClientProjectId);
            }

            var lstItem = objDS.Tables[0].AsEnumerable().Select(p => new List<string> {                        
                        p.Field<Int32>("ID").ToString(),
                        p.Field<string>("Queue").ToString(),
                        p.Field<Boolean>("IsActive").ToString()
                }).ToList();

            var json = JsonConvert.SerializeObject(lstItem).ToString();
            return json;
        }
        catch (Exception ex)
        {
            ExceptionHandler.Logger(ex, ExceptionHandler.MessageType.Exception, ExceptionHandler.LogType.Web);
        }

        return "";
    }

    [WebMethod(EnableSession = true)]
    public static string InsertUpdateQueue(string Flag,int QueueId, string Queuename, bool QueueIsActive)
    {
        int retval;
        string retstr;
        try
        {
            ProfileCommon profile = ((ProfileCommon)HttpContext.Current.Profile).GetProfile(HttpContext.Current.User.Identity.Name);

            using (var QueueBL = new EPS.DataLayer.SiteMaster())
            {
                var QueueObject = new EPS.Entities.Queue()
                {
                    ClientProjectId = profile.ClientProjectId,
                    QueueId = QueueId,
                    QueueName = Queuename,
                    isActive = QueueIsActive,
                    Flag =Flag,
                    ProjectKey =profile.ProjectKey,
                    ClientKey=profile.ClientKey
                };
               retval = QueueBL.insertorupdateQueue(QueueObject);
            }

            if (retval == 0)
            {
                retstr= "[\" 0 \"]";
            }
            else
            {
                retstr = "[\" 1 \"]";
            }
        }
        catch (ApplicationException)
        {
            return "[\" 2 \"]";
        }
        catch (Exception)
        {
            return "[\" 0 \"]";
        }

        return retstr;
    }
}