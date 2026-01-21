namespace NemtPlatform.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using NemtPlatform.Application.Contracts.MultiTenancy;
using NemtPlatform.Domain.Common;
using NemtPlatform.Domain.Entities;
using NemtPlatform.Domain.Entities.Billing;
using NemtPlatform.Domain.Entities.Compliance;
using NemtPlatform.Domain.Entities.Configuration;
using NemtPlatform.Domain.Entities.Execution;
using NemtPlatform.Domain.Entities.Fleet;
using NemtPlatform.Domain.Entities.Identity;
using NemtPlatform.Domain.Entities.Locations;
using NemtPlatform.Domain.Entities.Operations;
using NemtPlatform.Domain.Entities.Passengers;

/// <summary>
/// Primary database context for the NEMT Platform application.
/// Manages all entity types and provides database access for the application.
/// Implements multi-tenancy support and automatic audit tracking.
/// </summary>
public class NemtPlatformDbContext : DbContext
{
    private readonly ITenantContext? _tenantContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="NemtPlatformDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    /// <param name="tenantContext">Optional tenant context for multi-tenancy support.
    /// If provided, enables automatic tenant filtering and TenantId population.</param>
    public NemtPlatformDbContext(
        DbContextOptions<NemtPlatformDbContext> options,
        ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    #region Core

    /// <summary>
    /// Gets the DbSet for Tenant entities.
    /// Represents master accounts for companies licensing the software.
    /// </summary>
    public DbSet<Tenant> Tenants => Set<Tenant>();

    #endregion

    #region Identity

    /// <summary>
    /// Gets the DbSet for User entities.
    /// Represents system users with authentication credentials.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets the DbSet for Employee entities.
    /// Represents staff members employed by the tenant organization.
    /// </summary>
    public DbSet<Employee> Employees => Set<Employee>();

    /// <summary>
    /// Gets the DbSet for DriverProfile entities.
    /// Contains driver-specific information extending employee data.
    /// </summary>
    public DbSet<DriverProfile> DriverProfiles => Set<DriverProfile>();

    /// <summary>
    /// Gets the DbSet for Role entities.
    /// Defines permission groups for authorization.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Gets the DbSet for Permission entities.
    /// Individual permissions that can be assigned to roles.
    /// </summary>
    public DbSet<Permission> Permissions => Set<Permission>();

    /// <summary>
    /// Gets the DbSet for PartnerUser entities.
    /// Represents users from partner organizations with limited access.
    /// </summary>
    public DbSet<PartnerUser> PartnerUsers => Set<PartnerUser>();

    #endregion

    #region Passengers

    /// <summary>
    /// Gets the DbSet for Passenger entities.
    /// Core passenger information for individuals requiring transportation.
    /// </summary>
    public DbSet<Passenger> Passengers => Set<Passenger>();

    /// <summary>
    /// Gets the DbSet for PatientProfile entities.
    /// Medical patient-specific information and requirements.
    /// </summary>
    public DbSet<PatientProfile> PatientProfiles => Set<PatientProfile>();

    /// <summary>
    /// Gets the DbSet for StudentProfile entities.
    /// Student-specific information for school transportation.
    /// </summary>
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();

    /// <summary>
    /// Gets the DbSet for Guardian entities.
    /// Legal guardians or authorized representatives for passengers.
    /// </summary>
    public DbSet<Guardian> Guardians => Set<Guardian>();

    /// <summary>
    /// Gets the DbSet for GuardianPassengerLink entities.
    /// Links guardians to their associated passengers.
    /// </summary>
    public DbSet<GuardianPassengerLink> GuardianPassengerLinks => Set<GuardianPassengerLink>();

    /// <summary>
    /// Gets the DbSet for EmergencyContact entities.
    /// Emergency contact information for passengers.
    /// </summary>
    public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();

    /// <summary>
    /// Gets the DbSet for TripCompanion entities.
    /// Companions accompanying passengers during trips.
    /// </summary>
    public DbSet<TripCompanion> TripCompanions => Set<TripCompanion>();

    #endregion

    #region Locations

    /// <summary>
    /// Gets the DbSet for Place entities.
    /// Named locations used as trip origins and destinations.
    /// </summary>
    public DbSet<Place> Places => Set<Place>();

    /// <summary>
    /// Gets the DbSet for AccessPoint entities.
    /// Specific pickup/dropoff points within or near places.
    /// </summary>
    public DbSet<AccessPoint> AccessPoints => Set<AccessPoint>();

    /// <summary>
    /// Gets the DbSet for Region entities.
    /// Geographic service areas for operational planning.
    /// </summary>
    public DbSet<Region> Regions => Set<Region>();

    #endregion

    #region Operations

    /// <summary>
    /// Gets the DbSet for Trip entities.
    /// Individual transportation requests for passengers.
    /// </summary>
    public DbSet<Trip> Trips => Set<Trip>();

    /// <summary>
    /// Gets the DbSet for Route entities.
    /// Planned sequences of trips assigned to vehicles and drivers.
    /// </summary>
    public DbSet<Route> Routes => Set<Route>();

    /// <summary>
    /// Gets the DbSet for RouteManifest entities.
    /// Detailed trip sequences with timing for route execution.
    /// </summary>
    public DbSet<RouteManifest> RouteManifests => Set<RouteManifest>();

    /// <summary>
    /// Gets the DbSet for Shift entities.
    /// Scheduled work periods for employees.
    /// </summary>
    public DbSet<Shift> Shifts => Set<Shift>();

    /// <summary>
    /// Gets the DbSet for ShiftSession entities.
    /// Actual clock-in/clock-out records for shifts.
    /// </summary>
    public DbSet<ShiftSession> ShiftSessions => Set<ShiftSession>();

    /// <summary>
    /// Gets the DbSet for Journey entities.
    /// Complete navigation sessions from vehicle assignment to return.
    /// </summary>
    public DbSet<Journey> Journeys => Set<Journey>();

    /// <summary>
    /// Gets the DbSet for StandingOrder entities.
    /// Recurring trip templates that generate scheduled trips.
    /// </summary>
    public DbSet<StandingOrder> StandingOrders => Set<StandingOrder>();

    #endregion

    #region Fleet

    /// <summary>
    /// Gets the DbSet for Vehicle entities.
    /// Transportation vehicles in the fleet.
    /// </summary>
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    /// <summary>
    /// Gets the DbSet for VehicleCredential entities.
    /// Registration, insurance, and compliance documents for vehicles.
    /// </summary>
    public DbSet<VehicleCredential> VehicleCredentials => Set<VehicleCredential>();

    /// <summary>
    /// Gets the DbSet for Equipment entities.
    /// Medical and safety equipment available in vehicles.
    /// </summary>
    public DbSet<Equipment> Equipment => Set<Equipment>();

    /// <summary>
    /// Gets the DbSet for MaintenanceRecord entities.
    /// Service and repair history for vehicles.
    /// </summary>
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();

    /// <summary>
    /// Gets the DbSet for VehicleInspection entities.
    /// Pre-shift and post-shift vehicle inspection records.
    /// </summary>
    public DbSet<VehicleInspection> VehicleInspections => Set<VehicleInspection>();

    /// <summary>
    /// Gets the DbSet for FuelLog entities.
    /// Fuel purchase and consumption tracking.
    /// </summary>
    public DbSet<FuelLog> FuelLogs => Set<FuelLog>();

    /// <summary>
    /// Gets the DbSet for VehicleTelemetry entities.
    /// Real-time vehicle location and status data.
    /// </summary>
    public DbSet<VehicleTelemetry> VehicleTelemetry => Set<VehicleTelemetry>();

    #endregion

    #region Billing

    /// <summary>
    /// Gets the DbSet for FundingSource entities.
    /// Payment sources like Medicaid, Medicare, or insurance plans.
    /// </summary>
    public DbSet<FundingSource> FundingSources => Set<FundingSource>();

    /// <summary>
    /// Gets the DbSet for Partner entities.
    /// Organizations that refer passengers for transportation services.
    /// </summary>
    public DbSet<Partner> Partners => Set<Partner>();

    /// <summary>
    /// Gets the DbSet for PartnerContract entities.
    /// Agreements and billing arrangements with partners.
    /// </summary>
    public DbSet<PartnerContract> PartnerContracts => Set<PartnerContract>();

    /// <summary>
    /// Gets the DbSet for Authorization entities.
    /// Pre-approved trip authorizations from funding sources.
    /// </summary>
    public DbSet<Authorization> Authorizations => Set<Authorization>();

    /// <summary>
    /// Gets the DbSet for EligibilityRecord entities.
    /// Passenger eligibility verification records for funding sources.
    /// </summary>
    public DbSet<EligibilityRecord> EligibilityRecords => Set<EligibilityRecord>();

    /// <summary>
    /// Gets the DbSet for ServiceCode entities.
    /// Billing codes for different types of transportation services.
    /// </summary>
    public DbSet<ServiceCode> ServiceCodes => Set<ServiceCode>();

    /// <summary>
    /// Gets the DbSet for Claim entities.
    /// Billing claims submitted to funding sources for reimbursement.
    /// </summary>
    public DbSet<Claim> Claims => Set<Claim>();

    /// <summary>
    /// Gets the DbSet for Remittance entities.
    /// Payment records from funding sources for submitted claims.
    /// </summary>
    public DbSet<Remittance> Remittances => Set<Remittance>();

    #endregion

    #region Execution

    /// <summary>
    /// Gets the DbSet for TripExecution entities.
    /// Real-time execution tracking for trips including status transitions.
    /// </summary>
    public DbSet<TripExecution> TripExecutions => Set<TripExecution>();

    /// <summary>
    /// Gets the DbSet for Incident entities.
    /// Records of accidents, delays, or other operational incidents.
    /// </summary>
    public DbSet<Incident> Incidents => Set<Incident>();

    /// <summary>
    /// Gets the DbSet for AuditLog entities.
    /// Comprehensive audit trail of system actions and changes.
    /// </summary>
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    /// <summary>
    /// Gets the DbSet for Document entities.
    /// File attachments and documents associated with various entities.
    /// </summary>
    public DbSet<Document> Documents => Set<Document>();

    #endregion

    #region Compliance

    /// <summary>
    /// Gets the DbSet for Credential entities.
    /// Credential type definitions for certifications and licenses.
    /// </summary>
    public DbSet<Credential> Credentials => Set<Credential>();

    /// <summary>
    /// Gets the DbSet for EmployeeCredential entities.
    /// Individual credential records for employees with expiration tracking.
    /// </summary>
    public DbSet<EmployeeCredential> EmployeeCredentials => Set<EmployeeCredential>();

    /// <summary>
    /// Gets the DbSet for AttributeDefinition entities.
    /// Configurable attribute types for extensible data capture.
    /// </summary>
    public DbSet<AttributeDefinition> AttributeDefinitions => Set<AttributeDefinition>();

    /// <summary>
    /// Gets the DbSet for DriverAttribute entities.
    /// Custom attribute values for drivers based on attribute definitions.
    /// </summary>
    public DbSet<DriverAttribute> DriverAttributes => Set<DriverAttribute>();

    /// <summary>
    /// Gets the DbSet for InspectionTemplate entities.
    /// Templates defining inspection checklists and procedures.
    /// </summary>
    public DbSet<InspectionTemplate> InspectionTemplates => Set<InspectionTemplate>();

    #endregion

    #region Configuration

    /// <summary>
    /// Gets the DbSet for ProcedureDefinition entities.
    /// Reusable procedure templates for operational workflows.
    /// </summary>
    public DbSet<ProcedureDefinition> ProcedureDefinitions => Set<ProcedureDefinition>();

    /// <summary>
    /// Gets the DbSet for ProcedureSet entities.
    /// Collections of procedures grouped for specific scenarios.
    /// </summary>
    public DbSet<ProcedureSet> ProcedureSets => Set<ProcedureSet>();

    /// <summary>
    /// Gets the DbSet for FormConfiguration entities.
    /// Dynamic form definitions for data collection.
    /// </summary>
    public DbSet<FormConfiguration> FormConfigurations => Set<FormConfiguration>();

    /// <summary>
    /// Gets the DbSet for ViewConfiguration entities.
    /// User interface view and layout configurations.
    /// </summary>
    public DbSet<ViewConfiguration> ViewConfigurations => Set<ViewConfiguration>();

    #endregion

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in DbSet properties on the context.
    /// Applies entity configurations from assembly and sets up multi-tenancy filters.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all IEntityTypeConfiguration implementations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NemtPlatformDbContext).Assembly);

        // Ignore value object types that are only used within JSON columns to prevent
        // EF Core from trying to configure them as separate entities. These types are
        // serialized as part of the parent's JSON column and don't need entity configuration.

        // Journey and Standing Order related
        modelBuilder.Ignore<Domain.Common.ValueObjects.JourneyLeg>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.LegTransition>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.JourneyTemplate>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.JourneyLegTemplate>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.StopTemplate>();

        // Procedure related
        modelBuilder.Ignore<Domain.Common.ValueObjects.ProcedureRule>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.ProcedureOverrides>();

        // Constraints related
        modelBuilder.Ignore<Domain.Common.ValueObjects.TripConstraints>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.ConstraintSet>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.DriverConstraints>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.VehicleConstraints>();

        // Directions and route related
        modelBuilder.Ignore<Domain.Common.ValueObjects.DirectionsData>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.Distance>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.Duration>();

        // View and configuration related
        modelBuilder.Ignore<Domain.Common.ValueObjects.ViewPreferences>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.SortingState>();

        // Compliance and inspection related
        modelBuilder.Ignore<Domain.Common.ValueObjects.ChecklistItem>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.InspectionDefect>();

        // Billing related
        modelBuilder.Ignore<Domain.Common.ValueObjects.ClaimLineItem>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.RemittanceAdjustment>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.ServiceLevelAgreement>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.AuthorizationLimits>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.AuthorizedDestination>();

        // Fleet related
        modelBuilder.Ignore<Domain.Common.ValueObjects.MedicalCapabilities>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.MaintenanceCosts>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.GeoJsonPolygon>();

        // Identity related
        modelBuilder.Ignore<Domain.Common.ValueObjects.DriverSkill>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.DriverPerformanceMetrics>();

        // Shift and operations related
        modelBuilder.Ignore<Domain.Common.ValueObjects.ShiftPersonnel>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.TimeWindow>();

        // Stop and execution related
        modelBuilder.Ignore<Domain.Entities.Execution.StopReconciliation>();
        modelBuilder.Ignore<Domain.Entities.Operations.BaseStop>();
        modelBuilder.Ignore<Domain.Entities.Operations.PassengerStop>();
        modelBuilder.Ignore<Domain.Entities.Operations.DriverServiceStop>();

        // Passenger related
        modelBuilder.Ignore<Domain.Common.ValueObjects.NotificationSettings>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.NotificationPreferences>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.QuietHours>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.GuardianPermissions>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.Verification>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.HandOffRecipient>();
        modelBuilder.Ignore<Domain.Common.ValueObjects.ScannedData>();

        // Note: Global query filters for multi-tenancy are applied in individual
        // entity configurations rather than here to allow for more granular control
        // and easier testing of tenant isolation.
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// Automatically sets audit fields (CreatedAt, UpdatedAt) and TenantId for new entities.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation.
    /// The task result contains the number of state entries written to the database.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set audit timestamps for all auditable entities
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }

        // Automatically set TenantId for new tenant entities if tenant context is available
        if (_tenantContext?.TenantId != null)
        {
            foreach (var entry in ChangeTracker.Entries<TenantEntity>())
            {
                if (entry.State == EntityState.Added && string.IsNullOrEmpty(entry.Entity.TenantId))
                {
                    entry.Entity.TenantId = _tenantContext.TenantId;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
