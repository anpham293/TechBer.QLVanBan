using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityParameters;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using TechBer.ChuyenDoiSo.Auditing.Dto;
using TechBer.ChuyenDoiSo.Authorization.Accounts.Dto;
using TechBer.ChuyenDoiSo.Authorization.Delegation;
using TechBer.ChuyenDoiSo.Authorization.Permissions.Dto;
using TechBer.ChuyenDoiSo.Authorization.Roles;
using TechBer.ChuyenDoiSo.Authorization.Roles.Dto;
using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.Authorization.Users.Delegation.Dto;
using TechBer.ChuyenDoiSo.Authorization.Users.Dto;
using TechBer.ChuyenDoiSo.Authorization.Users.Importing.Dto;
using TechBer.ChuyenDoiSo.Authorization.Users.Profile.Dto;
using TechBer.ChuyenDoiSo.Chat;
using TechBer.ChuyenDoiSo.Chat.Dto;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;
using TechBer.ChuyenDoiSo.Editions;
using TechBer.ChuyenDoiSo.Editions.Dto;
using TechBer.ChuyenDoiSo.Friendships;
using TechBer.ChuyenDoiSo.Friendships.Cache;
using TechBer.ChuyenDoiSo.Friendships.Dto;
using TechBer.ChuyenDoiSo.Localization.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy;
using TechBer.ChuyenDoiSo.MultiTenancy.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments.Dto;
using TechBer.ChuyenDoiSo.Notifications.Dto;
using TechBer.ChuyenDoiSo.Organizations.Dto;
using TechBer.ChuyenDoiSo.Sessions.Dto;
using TechBer.ChuyenDoiSo.WebHooks.Dto;

namespace TechBer.ChuyenDoiSo
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditUserDuAnDto, UserDuAn>().ReverseMap();
            configuration.CreateMap<UserDuAnDto, UserDuAn>().ReverseMap();
            configuration.CreateMap<CreateOrEditBaoCaoVanBanDuAnDto, BaoCaoVanBanDuAn>().ReverseMap();
            configuration.CreateMap<BaoCaoVanBanDuAnDto, BaoCaoVanBanDuAn>().ReverseMap();
            configuration.CreateMap<CreateOrEditTraoDoiVanBanDuAnDto, TraoDoiVanBanDuAn>().ReverseMap();
            configuration.CreateMap<TraoDoiVanBanDuAnDto, TraoDoiVanBanDuAn>().ReverseMap();
            configuration.CreateMap<CreateOrEditQuyetDinhDto, QuyetDinh>().ReverseMap();
            configuration.CreateMap<QuyetDinhDto, QuyetDinh>().ReverseMap();
            configuration.CreateMap<CreateOrEditThungHoSoDto, ThungHoSo>().ReverseMap();
            configuration.CreateMap<ThungHoSoDto, ThungHoSo>().ReverseMap();
            configuration.CreateMap<CreateOrEditDayKeDto, DayKe>().ReverseMap();
            configuration.CreateMap<DayKeDto, DayKe>().ReverseMap();
            configuration.CreateMap<CreateOrEditPhongKhoDto, PhongKho>().ReverseMap();
            configuration.CreateMap<PhongKhoDto, PhongKho>().ReverseMap();
            configuration.CreateMap<CreateOrEditChuyenHoSoGiayDto, ChuyenHoSoGiay>().ReverseMap();
            configuration.CreateMap<ChuyenHoSoGiayDto, ChuyenHoSoGiay>().ReverseMap();
            configuration.CreateMap<CreateOrEditLoaiKhoanDto, LoaiKhoan>().ReverseMap();
            configuration.CreateMap<LoaiKhoanDto, LoaiKhoan>().ReverseMap();
            configuration.CreateMap<CreateOrEditChuongDto, Chuong>().ReverseMap();
            configuration.CreateMap<ChuongDto, Chuong>().ReverseMap();
            configuration.CreateMap<CreateOrEditCapQuanLyDto, CapQuanLy>().ReverseMap();
            configuration.CreateMap<CapQuanLyDto, CapQuanLy>().ReverseMap();
            configuration.CreateMap<CreateOrEditQuyTrinhDuAnAssignedDto, QuyTrinhDuAnAssigned>().ReverseMap();
            configuration.CreateMap<QuyTrinhDuAnAssignedDto, QuyTrinhDuAnAssigned>().ReverseMap();
            configuration.CreateMap<CreateOrEditVanBanDuAnDto, VanBanDuAn>().ReverseMap();
            configuration.CreateMap<VanBanDuAnDto, VanBanDuAn>().ReverseMap();
            configuration.CreateMap<CreateOrEditDuAnDto, DuAn>().ReverseMap();
            configuration.CreateMap<DuAnDto, DuAn>().ReverseMap();
            configuration.CreateMap<CreateOrEditQuyTrinhDuAnDto, QuyTrinhDuAn>().ReverseMap();
            configuration.CreateMap<QuyTrinhDuAnDto, QuyTrinhDuAn>().ReverseMap();
            configuration.CreateMap<CreateOrEditLoaiDuAnDto, LoaiDuAn>().ReverseMap();
            configuration.CreateMap<LoaiDuAnDto, LoaiDuAn>().ReverseMap();
            configuration.CreateMap<CreateOrEditChiTietDanhGiaDto, ChiTietDanhGia>().ReverseMap();
            configuration.CreateMap<ChiTietDanhGiaDto, ChiTietDanhGia>().ReverseMap();
            configuration.CreateMap<CreateOrEditTieuChiDanhGiaDto, TieuChiDanhGia>().ReverseMap();
            configuration.CreateMap<TieuChiDanhGiaDto, TieuChiDanhGia>().ReverseMap();
            configuration.CreateMap<CreateOrEditQuanHuyenDto, QuanHuyen>().ReverseMap();
            configuration.CreateMap<QuanHuyenDto, QuanHuyen>().ReverseMap();
            configuration.CreateMap<CreateOrEditTinhThanhDto, TinhThanh>().ReverseMap();
            configuration.CreateMap<TinhThanhDto, TinhThanh>().ReverseMap();
            configuration.CreateMap<CreateOrEditDoiTuongChuyenDoiSoDto, DoiTuongChuyenDoiSo>().ReverseMap();
            configuration.CreateMap<DoiTuongChuyenDoiSoDto, DoiTuongChuyenDoiSo>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicParameter, DynamicParameterDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueDto>().ReverseMap();
            configuration.CreateMap<EntityDynamicParameter, EntityDynamicParameterDto>()
                .ForMember(dto => dto.DynamicParameterName,
                    options => options.MapFrom(entity => entity.DynamicParameter.ParameterName));
            configuration.CreateMap<EntityDynamicParameterDto, EntityDynamicParameter>();

            configuration.CreateMap<EntityDynamicParameterValue, EntityDynamicParameterValueDto>().ReverseMap();
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();


            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
