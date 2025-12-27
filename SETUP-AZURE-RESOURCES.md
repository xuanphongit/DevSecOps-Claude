# Azure Resources Setup for Task 1.3

## 💰 Cost Optimization Strategy

**Rule**: Always prioritize FREE solutions for learning/development.

---

## ✅ FREE Solution: GitHub Container Registry (GHCR)

**Status**: ✅ Implemented in workflow

### Why GHCR?
- ✅ **100% FREE** (unlimited for public repos, generous limits for private)
- ✅ No Azure costs
- ✅ Integrated with GitHub
- ✅ Works perfectly for CI/CD
- ✅ Supports image scanning (GitHub Advanced Security)

### Setup Required
**NONE** - GitHub Actions automatically has access via `GITHUB_TOKEN`!

### Image Location
- Registry: `ghcr.io`
- Full path: `ghcr.io/{username}/secureshop-api:latest`

---

## 📋 Azure Resources Status

### ⚠️ ACR Not Needed for Phase 1
- **Decision**: Using GitHub Container Registry (FREE) instead
- **Reason**: Cost optimization - no need for paid ACR in learning phase
- **ACR can be created later** when needed for production/advanced scenarios

### If ACR Needed Later (Optional)
- **Resource Group**: `rg-secureshop-dev` (can be created when needed)
- **ACR Basic**: ~$5/month (only create if absolutely necessary)
- **See**: `docs/ACR-PRICING-GUIDE.md` for detailed pricing breakdown

---

## 🔄 Workflow Configuration

The CI pipeline now uses:
- **Registry**: `ghcr.io` (GitHub Container Registry)
- **Authentication**: Automatic via `GITHUB_TOKEN`
- **Cost**: $0

### To View Images
1. Go to GitHub repo → Packages
2. Images will appear as: `secureshop-api`

### To Pull Images Locally
```bash
# Login to GHCR
echo $GITHUB_TOKEN | docker login ghcr.io -u USERNAME --password-stdin

# Pull image
docker pull ghcr.io/{username}/secureshop-api:latest
```

---

## 💡 Cost Comparison

| Solution | Cost | Setup Complexity |
|----------|------|------------------|
| **GitHub Container Registry** | **FREE** | ✅ Automatic |
| Azure Container Registry Basic | ~$5/month | ⚠️ Requires setup |
| Docker Hub (free tier) | FREE (limited) | ⚠️ Requires account |

**Winner**: GitHub Container Registry - FREE and automatic! ✅

---

## ✅ Next Steps

1. ✅ Workflow configured for GHCR
2. ✅ No Azure resources needed
3. ✅ Push code to trigger pipeline
4. ✅ Images will be published to GHCR automatically

---

*Last Updated: December 26, 2024*  
*Strategy: Always prioritize FREE solutions*
