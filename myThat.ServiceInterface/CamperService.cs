using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using SharpStack.Data.UnitsOfWork;
using SharpStack.Services.UnitsOfWork;
using myThat.ServiceInterface.QueryObjects;
using myThat.ServiceModel.Data;
using myThat.ServiceModel.Request;
using ServiceStack.Common.Utils;
using System.Drawing;



namespace myThat.ServiceInterface
{
    public class CamperService : Service
    {
        public Camper Get(CamperProfile request)
        {
            using (var uow = GetUnitOfWork())
            {
                var sess = base.SessionAs<AuthUserSession>();
                var camper = GetCamperQueryObject(uow).RestrictByEmail(sess.Email).GetSingle();

                return camper;
            }
        }

        public Camper Put(EditCamper request)
        {
            using (var uow = GetUnitOfWork())
            {
                var camperQueryObject = GetCamperQueryObject(uow);
                var camper = camperQueryObject.RestrictByEmail(request.Email).GetSingle();
                camper.PopulateWithNonDefaultValues(request);
                uow.Save(camper);
                //Save image down
                foreach (var uploadedFile in RequestContext.Files.Where(uploadedFile => uploadedFile.ContentLength > 0))
                {
                    using (var ms = new MemoryStream())
                    {
                        var fileType = Path.GetExtension(uploadedFile.FileName);
                        uploadedFile.WriteTo(ms);
                        WriteImage(ms, camper.Id + "." + fileType);
                    }
                }

                uow.CommitTransaction();
                return camper;
            }
        }

        public virtual IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork("");
        }

        public virtual CamperQueryObject GetCamperQueryObject(IUnitOfWork uow)
        {
            return new CamperQueryObject(uow);
        }

        private void WriteImage(Stream ms, string imageName)
        {
            var uploadsDir = "~/Images/campers".MapHostAbsolutePath();
            ms.Position = 0;
            using (var img = Image.FromStream(ms))
            {
                img.Save(uploadsDir.CombineWith(imageName));
            }
        }
    }
}
