# 🏗️ Phase 3: IaC Security & Policy as Code

**Status**: Future Phase (Months 9-12)  
**Reference**: Use this file when working on Phase 3 tasks

---

## 🎯 Phase 3 Focus

- Terraform/Bicep security patterns
- Azure Policy implementation
- Landing Zone architecture
- Automated compliance checking
- Policy-driven governance

---

## 🏗️ Terraform Security Requirements

### Azure Resources
```yaml
NEVER:
  - Create public endpoints by default
  - Use shared access keys long-term
  - Skip encryption at rest
  - Ignore NSG rules
  - Use overly permissive RBAC

ALWAYS:
  - Use Private Endpoints for Azure services
  - Enable encryption at rest and in transit
  - Implement NSG with least privilege
  - Use Azure Policy for governance
  - Enable Defender for Cloud
  - Implement audit logging
```

---

## 📋 Terraform Templates

### AKS Cluster (Full Security)
```hcl
resource "azurerm_kubernetes_cluster" "aks" {
  name                = var.cluster_name
  location            = var.location
  resource_group_name = var.resource_group_name
  dns_prefix          = var.dns_prefix
  kubernetes_version  = var.kubernetes_version
  
  identity {
    type = "SystemAssigned"
  }
  
  default_node_pool {
    name                = "system"
    node_count          = var.system_node_count
    vm_size             = var.system_node_size
    type                = "VirtualMachineScaleSets"
    enable_auto_scaling = true
    min_count           = var.system_node_min_count
    max_count           = var.system_node_max_count
    os_disk_type        = "Ephemeral"
    vnet_subnet_id      = var.subnet_id
  }
  
  network_profile {
    network_plugin     = "azure"
    network_policy     = "calico"
    load_balancer_sku  = "standard"
    service_cidr       = var.service_cidr
    dns_service_ip     = var.dns_service_ip
  }
  
  role_based_access_control_enabled = true
  
  azure_active_directory_role_based_access_control {
    managed                = true
    admin_group_object_ids = var.admin_group_object_ids
    azure_rbac_enabled     = true
  }
  
  api_server_access_profile {
    authorized_ip_ranges = var.authorized_ip_ranges
  }
  
  private_cluster_enabled = var.private_cluster_enabled
  
  key_vault_secrets_provider {
    secret_rotation_enabled  = true
    secret_rotation_interval = "2m"
  }
  
  azure_policy_enabled = true
  
  microsoft_defender {
    log_analytics_workspace_id = var.log_analytics_workspace_id
  }
  
  oms_agent {
    log_analytics_workspace_id = var.log_analytics_workspace_id
  }
  
  tags = merge(
    var.tags,
    {
      "environment" = var.environment
      "managed-by"  = "terraform"
      "security"    = "compliant"
    }
  )
}
```

### Storage Account (Encrypted)
```hcl
resource "azurerm_storage_account" "main" {
  name                     = var.storage_account_name
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  
  blob_properties {
    versioning_enabled = true
    delete_retention_policy {
      days = 30
    }
  }
  
  encryption {
    key_vault_key_id = azurerm_key_vault_key.main.id
    identity_id      = azurerm_user_assigned_identity.main.id
  }
}
```

### Network Security Group (Strict)
```hcl
resource "azurerm_network_security_group" "main" {
  name                = var.nsg_name
  location            = var.location
  resource_group_name = var.resource_group_name
  
  # Deny all inbound by default
  security_rule {
    name                       = "DenyAllInbound"
    priority                   = 4096
    direction                  = "Inbound"
    access                     = "Deny"
    protocol                   = "*"
    source_port_range          = "*"
    destination_port_range     = "*"
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }
}
```

---

*This file is for Phase 3 reference. Current focus: Phase 1 (CI/CD Security)*

