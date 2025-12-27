# 💰 Azure Container Registry (ACR) Pricing Guide

## 📊 Pricing Model Overview

Azure Container Registry tính phí theo **3 thành phần chính**:
1. **Registry Tier** (gói dịch vụ) - tính theo ngày
2. **Storage** (lưu trữ) - tính theo GB vượt quá included storage
3. **Operations** (thao tác) - một số operations có thể tính phí

---

## 🎯 Pricing Tiers

### Basic Tier
- **Giá**: $0.167/ngày (~$5/tháng)
- **Included Storage**: 10 GB
- **Included Webhooks**: 2 webhooks
- **Features**:
  - Image storage
  - Basic webhooks
  - No geo-replication
  - No content trust
  - No private endpoint

### Standard Tier
- **Giá**: $0.667/ngày (~$20/tháng)
- **Included Storage**: 100 GB
- **Included Webhooks**: 10 webhooks
- **Features**:
  - Tất cả features của Basic
  - Content trust (image signing)
  - Private endpoint support
  - No geo-replication

### Premium Tier
- **Giá**: $1.667/ngày (~$50/tháng)
- **Included Storage**: 500 GB
- **Included Webhooks**: 500 webhooks
- **Features**:
  - Tất cả features của Standard
  - Geo-replication (thêm $1.667/ngày cho mỗi region)
  - Zone redundancy
  - Advanced security features

---

## 💵 How Billing Works

### 1. Registry Tier (Daily Charge)
```
Cost = Tier Price × Number of Days
```

**Ví dụ**:
- Basic tier: $0.167 × 30 ngày = $5.01/tháng
- **Lưu ý**: Tính theo ngày, không theo giờ
  - Tạo lúc 11:59 PM → Xóa lúc 12:01 AM = Vẫn tính 1 ngày
  - Tạo lúc 12:01 AM → Xóa lúc 11:59 PM = Tính 1 ngày

### 2. Storage (Overage)
```
Storage Cost = (Total Storage - Included Storage) × $0.10/GB/month
```

**Ví dụ Basic Tier**:
- Included: 10 GB
- Actual usage: 15 GB
- Overage: 15 - 10 = 5 GB
- Cost: 5 GB × $0.10 = $0.50/tháng

**Ví dụ Standard Tier**:
- Included: 100 GB
- Actual usage: 120 GB
- Overage: 120 - 100 = 20 GB
- Cost: 20 GB × $0.10 = $2.00/tháng

### 3. Operations
- **Most operations are FREE**: Pull, push, delete, list
- **Some operations may have limits**: Check Azure pricing page for details

---

## 📈 Cost Examples

### Example 1: Basic Tier - Light Usage
```
Registry: Basic ($0.167/ngày)
Storage: 5 GB (within 10 GB included)
Webhooks: 1 webhook (within 2 included)

Total Cost: $0.167 × 30 = $5.01/tháng
```

### Example 2: Basic Tier - Over Storage Limit
```
Registry: Basic ($0.167/ngày)
Storage: 15 GB (5 GB over included 10 GB)
Webhooks: 2 webhooks (within limit)

Cost Breakdown:
- Registry: $0.167 × 30 = $5.01
- Storage Overage: 5 GB × $0.10 = $0.50
Total: $5.51/tháng
```

### Example 3: Standard Tier - Medium Usage
```
Registry: Standard ($0.667/ngày)
Storage: 50 GB (within 100 GB included)
Webhooks: 5 webhooks (within 10 included)

Total Cost: $0.667 × 30 = $20.01/tháng
```

### Example 4: Premium Tier - Geo-Replication
```
Registry: Premium ($1.667/ngày)
Storage: 200 GB (within 500 GB included)
Webhooks: 20 webhooks (within 500 included)
Geo-replication: 2 additional regions

Cost Breakdown:
- Registry: $1.667 × 30 = $50.01
- Geo-replication: $1.667 × 2 regions × 30 = $100.02
Total: $150.03/tháng
```

---

## ⚠️ Important Billing Notes

### 1. Daily Billing (Not Hourly)
- **Minimum charge**: 1 ngày
- Tạo và xóa trong cùng ngày → Vẫn tính 1 ngày
- **Best practice**: Nếu không dùng, xóa ngay để tránh tính phí ngày tiếp theo

### 2. Storage Calculation
- Storage tính theo **peak usage** trong tháng
- Không tính theo average
- **Tip**: Xóa old images để giảm storage cost

### 3. Partial Days
- Không có partial day billing
- Tạo lúc bất kỳ trong ngày → Tính cả ngày
- Xóa lúc bất kỳ trong ngày → Vẫn tính cả ngày

### 4. Regional Pricing
- Giá có thể khác nhau theo region
- US regions thường rẻ nhất
- Check Azure pricing calculator cho region cụ thể

---

## 💡 Cost Optimization Tips

### 1. Choose Right Tier
- **Learning/Dev**: Basic ($5/tháng) hoặc dùng GHCR (FREE)
- **Production (small)**: Basic hoặc Standard
- **Production (large)**: Premium với geo-replication

### 2. Manage Storage
- Xóa old/unused images thường xuyên
- Use image retention policies
- Monitor storage usage

### 3. Use Free Alternatives
- **GitHub Container Registry (GHCR)**: FREE cho public repos
- **Docker Hub**: FREE tier (limited)
- **Azure Container Instances**: Pay per use (không cần registry)

### 4. Delete When Not Needed
- Xóa registry ngay khi không dùng
- Tránh để registry "idle" → Vẫn tính phí

---

## 📊 Cost Comparison

| Solution | Cost | Storage | Best For |
|----------|------|---------|----------|
| **GitHub Container Registry** | **FREE** | Unlimited (public) | Learning, Open source |
| **Docker Hub (free)** | **FREE** | 1 private repo | Small projects |
| **ACR Basic** | ~$5/month | 10 GB included | Dev/Test, Small prod |
| **ACR Standard** | ~$20/month | 100 GB included | Medium production |
| **ACR Premium** | ~$50/month | 500 GB included | Large production |

---

## 🔍 How to Check Your ACR Costs

### Azure Portal
1. Go to **Cost Management + Billing**
2. Filter by resource: `Microsoft.ContainerRegistry`
3. View daily/monthly costs

### Azure CLI
```bash
# Check ACR usage
az acr show-usage --name <registry-name> --resource-group <rg-name>

# Check storage
az acr repository show --name <registry-name> --repository <repo-name>
```

### Azure Cost Management API
```bash
az consumption usage list --start-date 2024-01-01 --end-date 2024-01-31
```

---

## 📝 Summary

### Key Takeaways
1. **ACR Basic**: ~$5/tháng (minimum 1 ngày billing)
2. **Storage overage**: $0.10/GB/tháng (vượt quá included)
3. **Daily billing**: Không tính theo giờ
4. **Free alternative**: GitHub Container Registry (GHCR) - FREE

### For Phase 1 Learning
- **Recommendation**: Dùng **GitHub Container Registry (FREE)**
- **Reason**: 
  - Zero cost
  - Automatic setup
  - Sufficient for learning
  - Can switch to ACR later if needed

---

*Last Updated: December 26, 2024*  
*Source: [Azure Container Registry Pricing](https://azure.microsoft.com/en-us/pricing/details/container-registry/)*

