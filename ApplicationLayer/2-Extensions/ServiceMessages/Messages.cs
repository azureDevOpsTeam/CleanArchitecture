namespace ApplicationLayer.Extensions.ServiceMessages
{
    public static class CommonMessages
    {
        public static string Successful = "عملیات با موفقیت انجام شد";

        public static string Failed = "عملیات با خطا مواجه شد";

        public static string Duplicate = "رکورد تکراری";

        public static string NotFound = "با اطلاعات داده شده رکوردی یافت نشد";

        public static string Exist = "اطلاعات کاربر تکراری است";

        public static string ExistOrderId = "مرسوله های یک ناوگان باید از یک سفارش باشند";

        public static string ExistBarcode = "بارکد تکراری است";

        public static string VerificationCodeInvalid = "کد وارد شده معتبر نیست";

        public static string ExistPolygon = "نوع محدوده مورد نظر قبلا ثبت شده است";

        public static string IncorrectUser = "اطلاعات کاربری اشتباه است";

        public static string OperationCancel = "عملیات توسط کاربر لغو شده است";

        public static string RequestResetPassword = "درخواست تغییر رمز عبور";

        public static string IncorrectFormat = "فرمت وارده برای ورودی معتبر نیست";

        public static string IncorrectCurrentPassword = "رمز عبور فعلی معتبر نیست";

        public static string NotInRange = "اطلاعات وارد شده در محدوده معتبر نمیباشد ";

        public static string ConcurrencyIssue = "خطای همزمانی رخ داده است. دیتای مورد نظر ممکن است حذف و یا ویرایش شده باشد.";

        public static string HasReferenced = "این انتیتی دارای رفرنس در انتیتی های دیگر میباشد.";

        public static string DeserializedError = "در پروسه ی دیسریالایز کردن خطایی رخ داده است";

        public static string AccessCheckError = "در پروسه ی بررسی دسترسی خطایی رخ داده است";

        public static string InvalidToken = "توکن معتبر نمیباشد";

        public static string ExpiredToken = "توکن مورد نظر منقضی شده است";

        public static string RefreshNotRequired = "توکن قبلی همچنان معتبر میباشد . نیازی به رفرش کردن آن نیست";

        public static string ParcelRecivedFleet = "این درخواست عملیاتی شده است و امکان لغو وجود ندارد";

        public static string UserExist(string name) => $"{name} وارد شده تکراری است";
    }

    public static class RefreshTokenMessages
    {
        public static string ExpiredTokensCleanupFailed = "در پروسه ی پاک کردن توکن های منقضی شده، خطایی رخ داده است";
    }

    public static class CommonValidateMessages
    {
        public static string Required(string name = "{PropertyName}") => $"{name} باید وارد شود.";

        public static string TitleRequired(string name = "{PropertyName}") => $"عنوان {name} باید وارد شود.";

        public static string NotNull(string name = "{PropertyName}") => $"{name} نمیتواند خالی باشد.";

        public static string LengthExceed(string name, int length) => $" مقدار فیلد {name} نمیتواند بیشتر از {length} کاراکتر باشد.";

        public static string LengthExceed(int length) => $" مقدار فیلد {{PropertyName}} نمیتواند بیشتر از {length} کاراکتر باشد.";

        public static string AtLeastOneRequired(string name = "{PropertyName}") => $" حداقل یک {name} را انتخاب کنید.";

        public static string NotBothHasValue(string firstProp, string secondProp) => $"{firstProp} و {secondProp}، هر دو نمیتوانند مقدار داشته باشند.";

        public static string AtLeastOneHasValue(string firstProp, string secondProp) => $"{firstProp} یا {secondProp}، باید یکی از آنها حداقل مقدار داشته باشد.";

        public static string HaveEitherButNotBoth(string firstProp, string secondProp) => $"یکی از فیلدهای {firstProp} یا {secondProp}، باید مقدار داشته باشند";

        public static string MustBeGreaterThanZero(string firstProp = "{PropertyName}") => $"مقدار فیلد {firstProp} باید بزرگتر از صفر باشد";

        public static string StartWithOut(string name, string value) => $"{name} نباید با {value} شروع شود.";

        public static string MustHaveValue(string name = "{PropertyName}") => $"برای {name} باید مقدار وارد شود.";
    }

    public static class IdentityMessages
    {
        public static string IncorrectPassword = "نام کاربری یا کلمه عبور اشتباه است";

        public static string NotFound = "کاربری با اطلاعات وارد شده یافت نشد";

        public static string IncorrectMobile = "شماره وارد شده اشتباه است";

        public static string IncorrectEmail = "ایمیل وارد شده اشتباه است";

        public static string IncorrectSecurityCode = "کد  وارد شده معتبر نیست";

        public static string ChangePasswordNotChangeForUserInternal = " تغییر رمزعبور کاربران درون سازمانی در این بخش امکان پذیر نیست";
    }

    public static class NotificationMessageMessages
    {
        public static string SendEmailSuccess = "ارسال ایمیل با موفقیت انجام شد";

        public static string SendSmsSuccess = "ارسال پیامک با موفقیت انجام شد";

        public static string SendEmailFailed = "ارسال ایمیل با خطا مواجه شد";

        public static string SendSmsFailed = "ارسال پیامک با خطا مواجه شد";

        public static string SendOneTimePasswordEmailSubject = "به رونیکس خوش آمدید!";

        public static string SendOneTimePasswordEmail = "کد تایید شما برای ورود به سامانه : ";

        public static string SendOneTimePasswordMobile = "کد تایید شما برای ورود به سامانه : ";

        public static string SendOneTimePasswordEmailConfirm = "کد تایید برای ایمیل وارد شده ارسال شد";

        public static string SendOneTimePasswordEmailFailed = "خطا در ارسال ایمیل";

        public static string SendOneTimePasswordMobileConfirm = "پیامک  کد تایید به شماره موبایل وارد شده ارسال شد";

        public static string SendOneTimePasswordMobileFailed = "خطا در ارسال پیامک";
    }

    public static class LanguageMessages
    {
    }

    public static class CompanyMessages
    {
    }

    public static class ValidationErrorCodes
    {
        public static readonly string NotNull = "100";
        public static readonly string LengthExceed = "101";
        public static readonly string AtLeastOneRequired = "102";
        public static readonly string NotBothHasValue = "103";
        public static readonly string AtLeastOneHasValue = "104";
        public static readonly string HaveEitherButNotBoth = "105";
        public static readonly string MustBeGreaterThanZero = "106";
        public static readonly string MustHaveValue = "107";
        public static readonly string ConcurrencyIssue = "108";
        public static readonly string StartWithOutZero = "109";
    }

    public static class GroupMessages
    {
        public static string FailedInFetchingGroupList = "خطایی در پروسه ی گرفتن لیست گروه ها رخ داده است";
        public static string FailedInFetchingGroupByFilter = "در گرفتن گروه با فیلتر، خطایی رخ داده است";
        public static string FailedInFetchingGroupBySearchTerm = "در جستجوی گروه، خطایی رخ داده است";
    }
}