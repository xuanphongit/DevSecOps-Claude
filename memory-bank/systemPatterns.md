# System Patterns: Architecture & Technical Decisions

## 🏗️ Overall Architecture

### Learning Project Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    DevSecOps Pipeline                        │
├─────────────────────────────────────────────────────────────┤
│  Source Code → Build → Test → Scan → Sign → Deploy → Monitor│
│       ↓         ↓       ↓      ↓       ↓       ↓        ↓   │
│    GitHub    .NET    Unit   SAST    Cosign   AKS    Defender │
│              Build   Tests   SCA            Deploy   Falco   │
│                             Secrets                          │
│                             Container                        │
└─────────────────────────────────────────────────────────────┘
```

### Security Layers

```
┌─────────────────────────────────────────┐
│           Application Security           │
│  (SAST, Input Validation, Auth/AuthZ)   │
├─────────────────────────────────────────┤
│           Container Security             │
│  (Image Scanning, Signing, Hardening)   │
├─────────────────────────────────────────┤
│           Kubernetes Security            │
│  (RBAC, PSS, Network Policies, AC)      │
├─────────────────────────────────────────┤
│           Infrastructure Security        │
│  (IaC Scanning, Azure Policy, NSG)      │
├─────────────────────────────────────────┤
│           Supply Chain Security          │
│  (SBOM, SLSA, Dependency Scanning)      │
└─────────────────────────────────────────┘
```

---

## 📐 Design Patterns in Use

### 1. Shift-Left Security
**Pattern**: Move security testing earlier in SDLC
**Implementation**:
- Pre-commit hooks (detect-secrets)
- PR-level scanning (CodeQL, SonarCloud)
- Build-time container scanning (Trivy)

### 2. Security as Code
**Pattern**: Define security policies as version-controlled code
**Implementation**:
- Azure Policy as Bicep/Terraform
- Kyverno policies as YAML
- OPA policies as Rego

### 3. Defense in Depth
**Pattern**: Multiple security layers
**Implementation**:
- Network: NSG → WAF → Network Policies
- Identity: Azure AD → RBAC → Pod Identity
- Data: Encryption at rest → TLS → Key Vault

### 4. Zero Trust
**Pattern**: Never trust, always verify
**Implementation**:
- Managed Identity (no secrets)
- mTLS between services
- Least privilege RBAC

---

## 🔧 Key Technical Decisions

### Decision 1: CI/CD Platform
**Choice**: GitHub Actions  
**Rationale**:
- Better portfolio visibility
- Strong community/marketplace
- Good Azure integration via azure/login action
- Free for public repos

### Decision 2: Container Registry
**Choice**: Azure Container Registry (ACR)  
**Rationale**:
- Native Azure integration
- Built-in vulnerability scanning (Defender)
- Geo-replication for production
- Content trust (signing)

### Decision 3: Security Scanning Tools
**Choices**:
| Category | Tool | Rationale |
|----------|------|-----------|
| SAST | CodeQL + SonarCloud | Free, comprehensive |
| SCA | Snyk + Dependabot | Best-in-class, free tier |
| Container | Trivy | Fast, comprehensive, free |
| Secrets | TruffleHog + detect-secrets | Git history + pre-commit |
| SBOM | Syft | Industry standard, CycloneDX |
| Signing | Cosign | Sigstore ecosystem, keyless option |

### Decision 4: Kubernetes Security
**Choices**:
- **Admission Controller**: Kyverno (YAML-native, easier than OPA)
- **Runtime Security**: Falco (eBPF-based, cloud-native)
- **Policy**: Pod Security Standards (built-in)

---

## 📁 Project Structure Patterns

### Repository Layout
```
project-name/
├── .github/
│   ├── workflows/          # CI/CD pipelines
│   └── dependabot.yml      # Dependency updates
├── src/
│   └── Application/        # Source code
├── tests/
│   └── Unit/              # Test code
├── infra/
│   ├── terraform/         # IaC
│   └── kubernetes/        # K8s manifests
├── security/
│   ├── policies/          # Security policies
│   └── baselines/         # Security baselines
└── docs/
    └── architecture/      # ADRs, diagrams
```

### Pipeline Stages Pattern
```yaml
stages:
  - build        # Compile, test
  - scan         # SAST, SCA, secrets
  - package      # Docker build, sign
  - gate         # Security decision
  - deploy       # To environment
  - verify       # Post-deploy checks
```

---

## 🔄 Component Relationships

### Security Tool Integration
```
Code Commit
    ↓
Pre-commit (detect-secrets)
    ↓
PR Created
    ↓
┌───────────────────────────┐
│   Parallel Scanning       │
│  ┌─────┐ ┌─────┐ ┌─────┐ │
│  │SAST │ │ SCA │ │Secrets│ │
│  └──┬──┘ └──┬──┘ └──┬──┘ │
└─────┼───────┼───────┼────┘
      └───────┼───────┘
              ↓
      Security Gate
              ↓
      ┌───────────────┐
      │ Pass?         │
      │ Yes → Deploy  │
      │ No → Block    │
      └───────────────┘
```

### Data Flow
```
Source → Build Artifact → Container Image → Signed Image → Registry → AKS
                ↓               ↓                ↓
              SBOM          Scan Report      Signature
                ↓               ↓                ↓
           Storage         Dashboard        Verification
```

---

## 📏 Conventions

### Naming Conventions
- **Resources**: `{project}-{env}-{resource}` (e.g., `secureshop-dev-acr`)
- **Branches**: `feature/task-X.X-description`
- **Commits**: `<type>(<scope>): <subject>`

### Security Thresholds
```
Critical CVE: 0 allowed
High CVE: 0 allowed  
Medium CVE: < 5 allowed
SAST Critical: 0 allowed
Secrets: 0 allowed
Coverage: >= 70%
```

---

**Created**: December 26, 2024  
**Last Updated**: December 26, 2024
