﻿using System;
using Composite.C1Console.Security;
using Composite.Data.Hierarchy;
using Composite.Data.Hierarchy.DataAncestorProviders;
using Composite.Data.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;


namespace Composite.Data.Types
{
    /// <summary>    
    /// This data interface represents a administrative user in Composite C1. This can be used to query users through a <see cref="Composite.Data.DataConnection"/>. 
    /// </summary>
    [AutoUpdateble]
    [KeyPropertyName("Id")]
    [LabelPropertyName("Username")]
    [DataScope(DataScopeIdentifier.PublicName)]
    [ImmutableTypeId("{AA086DC1-E5F6-4568-8BED-460D3275380F}")]
    [Caching(CachingType.Full)]    
    [DataAncestorProvider(typeof(NoAncestorDataAncestorProvider))]
    [Title("C1 Console User")]
    public interface IUser : IData
    {
        /// <exclude />
        [StoreFieldType(PhysicalStoreFieldType.Guid)]
        [ImmutableFieldId("{480CF63A-A2E9-43e7-8034-778E55A0B6ED}")]
        Guid Id { get; set; }


        /// <exclude />
        [NotNullValidator()]
        [StringSizeValidator(2, 64)]
        [StoreFieldType(PhysicalStoreFieldType.String, 64)]
        [ImmutableFieldId("{C36F8A02-4F7E-437c-A3D9-AADE8A531EFA}")]
        string Username { get; set; }


        /// <exclude />
        [StoreFieldType(PhysicalStoreFieldType.String, 128, IsNullable = true)]
        [ImmutableFieldId("{20F756AD-F03D-453E-B464-D2C13051A647}")]
        string Name { get; set; }


        /// <exclude />
        [StoreFieldType(PhysicalStoreFieldType.String, 128, IsNullable = true)]
        [ImmutableFieldId("{C0929E14-3FB8-4CE5-8374-997A7508DF80}")]
        string Email { get; set; }


        /// <exclude />
        [NotNullValidator()]
        [StringSizeValidator(1, 64)]
        [StoreFieldType(PhysicalStoreFieldType.String, 64)]
        [ImmutableFieldId("{C7A7D63C-EA87-48ac-B009-5D4050A2F248}")]
        // This should be named Folder and have no relation to IUserGroup
        string Group { get; set; }

        /// <summary>
        /// Determines whether the user is locked
        /// </summary>
        [StoreFieldType(PhysicalStoreFieldType.Boolean)]
        [DefaultFieldBoolValue(false)]
        [ImmutableFieldId("{72BFFBDC-E4FF-4BD4-9BB1-B86FAEA39468}")]
        bool IsLocked { get; set; }

        /// <summary>
        /// Contains a code describing lockout reason (f.e. locked by an administrator or automatically after X failed login attempts)
        /// For possible values, see enumeration <see cref="UserLockoutReason"/>
        /// </summary>
        [StoreFieldType(PhysicalStoreFieldType.Integer)]
        [DefaultFieldIntValue(0)]
        [ImmutableFieldId("{39CBCCFA-CFB2-4E79-9204-C9596A3FC0E9}")]
        int LockoutReason { get; set; }

        /// <exclude />
        [NotNullValidator]
        [StoreFieldType(PhysicalStoreFieldType.String, 256)]
        [ImmutableFieldId("{C0230DEB-5394-4819-BE18-A60CF5FA69F0}")]
        string EncryptedPassword { get; set; }

        /// <exclude />
        [StoreFieldType(PhysicalStoreFieldType.String, 128, IsNullable = true)]
        [ImmutableFieldId("{CA2CF6F8-489B-4D60-B3C3-AF46D1259647}")]
        string PasswordHashSalt { get; set; }

        /// <exclude />
        [StoreFieldType(PhysicalStoreFieldType.DateTime)]
        [ImmutableFieldId("{35457DA7-A13E-4A6C-9008-3D619A519F2B}")]
        [DefaultFieldNowDateTimeValue]
        DateTime LastPasswordChangeDate { get; set; }
    }
}
