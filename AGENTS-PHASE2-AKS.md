# ☸️ Phase 2: Azure Kubernetes Security

**Status**: Future Phase (Months 3-9)  
**Reference**: Use this file when working on Phase 2 tasks

---

## 🎯 Phase 2 Focus

- AKS security baseline implementation
- RBAC and Pod Security Standards
- Network policies and segmentation
- Admission controllers (Kyverno/OPA)
- Runtime defense mechanisms

---

## ☸️ Kubernetes Security Requirements

### Pod Security Standards
```yaml
NEVER:
  - Use privileged containers
  - Mount host filesystem
  - Use default service accounts
  - Allow privilege escalation
  - Run without resource limits

ALWAYS:
  - Implement Pod Security Standards (restricted)
  - Use NetworkPolicies (default deny)
  - Implement RBAC least privilege
  - Set resource requests and limits
  - Use read-only root filesystem where possible
  - Define security context for all workloads
```

---

## 📋 Kubernetes Templates

### Secure Deployment (Full Template)
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: app-deployment
  labels:
    app: myapp
    version: v1
spec:
  replicas: 3
  selector:
    matchLabels:
      app: myapp
  template:
    metadata:
      labels:
        app: myapp
        version: v1
      annotations:
        cosign.sigstore.dev/signature: "required"
    spec:
      serviceAccountName: myapp-sa
      automountServiceAccountToken: false
      
      securityContext:
        runAsNonRoot: true
        runAsUser: 1001
        runAsGroup: 1001
        fsGroup: 1001
        seccompProfile:
          type: RuntimeDefault
      
      containers:
      - name: myapp
        image: acr.azurecr.io/myapp:v1.0.0
        imagePullPolicy: Always
        
        securityContext:
          allowPrivilegeEscalation: false
          runAsNonRoot: true
          runAsUser: 1001
          readOnlyRootFilesystem: true
          capabilities:
            drop:
              - ALL
        
        resources:
          requests:
            memory: "128Mi"
            cpu: "100m"
          limits:
            memory: "256Mi"
            cpu: "200m"
        
        livenessProbe:
          httpGet:
            path: /api/health
            port: 8080
          initialDelaySeconds: 10
          periodSeconds: 10
        
        readinessProbe:
          httpGet:
            path: /api/health
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
        
        volumeMounts:
        - name: secrets-store
          mountPath: "/mnt/secrets"
          readOnly: true
        - name: tmp
          mountPath: /tmp
      
      volumes:
      - name: secrets-store
        csi:
          driver: secrets-store.csi.k8s.io
          readOnly: true
          volumeAttributes:
            secretProviderClass: "myapp-secrets"
      - name: tmp
        emptyDir: {}
```

### NetworkPolicy (Default Deny)
```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: myapp-netpol
spec:
  podSelector:
    matchLabels:
      app: myapp
  policyTypes:
  - Ingress
  - Egress
  ingress:
  - from:
    - podSelector:
        matchLabels:
          app: ingress-nginx
    ports:
    - protocol: TCP
      port: 8080
  egress:
  - to:
    - podSelector:
        matchLabels:
          app: postgres
    ports:
    - protocol: TCP
      port: 5432
  - to:
    - namespaceSelector:
        matchLabels:
          name: kube-system
    ports:
    - protocol: TCP
      port: 53
    - protocol: UDP
      port: 53
```

### ServiceAccount & RBAC
```yaml
# ServiceAccount
apiVersion: v1
kind: ServiceAccount
metadata:
  name: myapp-sa
automountServiceAccountToken: false

# Role (Least Privilege)
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: myapp-role
rules:
- apiGroups: [""]
  resources: ["configmaps"]
  verbs: ["get", "list"]
- apiGroups: [""]
  resources: ["secrets"]
  verbs: ["get"]
  resourceNames: ["myapp-secret"]

# RoleBinding
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: myapp-rolebinding
roleRef:
  apiGroup: rbac.authorization.k8s.io
  kind: Role
  name: myapp-role
subjects:
- kind: ServiceAccount
  name: myapp-sa
```

---

*This file is for Phase 2 reference. Current focus: Phase 1 (CI/CD Security)*
