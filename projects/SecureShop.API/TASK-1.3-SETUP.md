# Task 1.3: CI Pipeline Setup - Azure Resources

## 📋 Azure Resources Status

### ✅ Already Created
- **Resource Group**: `rg-secureshop-dev` (eastus)
- **Azure Container Registry**: `acrsecureshopdev` (Basic SKU)
  - Login Server: `acrsecureshopdev.azurecr.io`

### ⏳ Resources Needed for GitHub Actions

Để GitHub Actions có thể push images lên ACR, cần tạo:

1. **Service Principal** (hoặc sử dụng Managed Identity)
   - Name: `github-actions-secureshop`
   - Role: Contributor (trên resource group)
   - Scope: `rg-secureshop-dev`

2. **GitHub Secret**: `AZURE_CREDENTIALS`
   - Chứa credentials của Service Principal
   - Format: JSON với clientId, clientSecret, subscriptionId, tenantId

---

## 🔧 Setup Instructions

### Option 1: Service Principal (Recommended for GitHub Actions)

**Command to create** (chạy local, không tự động):
```bash
az ad sp create-for-rbac \
  --name "github-actions-secureshop" \
  --role contributor \
  --scopes /subscriptions/346e15e4-2da2-4e26-8303-3a7f76b03587/resourceGroups/rg-secureshop-dev \
  --sdk-auth
```

**Output sẽ là JSON**, copy toàn bộ và:
1. Vào GitHub repo → Settings → Secrets and variables → Actions
2. Tạo secret mới: `AZURE_CREDENTIALS`
3. Paste JSON output vào

### Option 2: ACR Admin Credentials (Simpler, less secure)

Nếu không muốn tạo Service Principal, có thể dùng ACR admin credentials:

```bash
# Get ACR admin credentials
az acr credential show --name acrsecureshopdev --resource-group rg-secureshop-dev
```

Sau đó tạo GitHub secrets:
- `ACR_USERNAME`: Username từ output
- `ACR_PASSWORD`: Password từ output

Và update workflow để dùng:
```yaml
- name: Login to Azure Container Registry
  run: |
    echo "${{ secrets.ACR_PASSWORD }}" | docker login ${{ env.ACR_NAME }}.azurecr.io \
      -u ${{ secrets.ACR_USERNAME }} --password-stdin
```

---

## ⚠️ Cost Estimate

**Current Resources**:
- Resource Group: Free
- ACR Basic: ~$5/month (10GB storage, 10,000 webhook operations)

**Total**: ~$5/month

---

## ✅ Next Steps

1. **Choose authentication method** (Service Principal or ACR Admin)
2. **Create credentials** (tôi sẽ thông báo command trước khi chạy)
3. **Add GitHub secrets**
4. **Test pipeline** (push code để trigger)

---

*Bạn muốn dùng phương thức nào? Service Principal (recommended) hay ACR Admin (simpler)?*


