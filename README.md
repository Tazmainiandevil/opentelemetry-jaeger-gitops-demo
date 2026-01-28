# Modern Observability with Jaeger v2, OpenTelemetry & GitOps
A multi‑part technical series with a fully working reference implementation
This repository contains the complete, production‑aligned implementation that accompanies my technical series on building a modern observability platform using Jaeger v2, the OpenTelemetry Collector, OpenTelemetry Operator, and Argo CD, all deployed declaratively through GitOps.
The series walks through the architecture, the reasoning behind it, and the real engineering decisions required to run tracing reliably at scale.

![Kubernetes](https://img.shields.io/badge/Kubernetes-1.29+-326ce5?logo=kubernetes&logoColor=white)
![Argo CD](https://img.shields.io/badge/Argo%20CD-3.1+-EF7B4D?logo=argo&logoColor=white)
![OpenTelemetry](https://img.shields.io/badge/OpenTelemetry-Collector%20%7C%20Operator-563D7C?logo=opentelemetry&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![GitOps](https://img.shields.io/badge/GitOps-Argo%20CD%20%7C%20helm-00A3E0)

## 📚 Blog Series

Part 1 — Why Jaeger v2 + OpenTelemetry + GitOps
Concepts, architecture, and the shift toward Collector‑based tracing

🔗 [Read: Part 1](https://codingwithtaz.blog/2026/01/05/part-1-why-jaeger-v2-opentelemetry-and-gitops-belong-together/)

Part 2 — Deploying Jaeger v2 with the OpenTelemetry Collector
Hands‑on deployment of Jaeger v2 using the OpenTelemetry Collector and GitOps

🔗 [Read: Part 2](https://codingwithtaz.blog/2026/01/05/part-2-deploying-jaeger-v2-with-the-opentelemetry-collector/)

Part 3 — Auto‑Instrumenting .NET with the OpenTelemetry Operator
How the Operator injects the .NET auto‑instrumentation agent with zero code changes
🔗 [Read: Part 3](https://codingwithtaz.blog/2026/01/12/part-3-auto-instrumenting-net-with-opentelemetry/)

Part 4 — Building a Scalable, Multi‑Environment GitOps Architecture
ApplicationSets, sync waves, environment isolation, and repo structure
🔗 [Read: Part 4](https://codingwithtaz.blog/2026/01/19/part-4-building-a-scalable-multi-environment-gitops-architecture-with-argo-cd/)

Part 5 — Troubleshooting, Scaling & Production Hardening
Real‑world operational guidance for Jaeger v2 and OpenTelemetry
🔗 [Read: Part 5](https://codingwithtaz.blog/2026/01/26/part-5-troubleshooting-scaling-and-production-hardening/)


## 📐 Architecture Overview
This demo implements a modern tracing pipeline:

```shell
.NET App (auto-instrumented)
        │
        ▼
OpenTelemetry Collector (Jaeger v2)
  - OTLP gRPC/HTTP receivers
  - Batch processor
  - Jaeger exporter (memstore)
  - Jaeger Query + UI
        │
        ▼
Jaeger UI (16686)
```

Everything is deployed and reconciled through ArgoCD, ensuring:
- Declarative configuration
- Drift detection
- Automatic sync
- Reproducible environments

## 🧰 Prerequisites
To deploy this reference implementation, you’ll need:
- Kubernetes 1.29+
- kubectl installed and configured
- Argo CD 3.1+ installed in the cluster
- Cluster admin permissions (for CRDs, webhooks, and platform components)
- A working ingress or port‑forward access (for Jaeger UI)
- A GitOps workflow (Argo CD watches this repo)
Everything else — cert-manager, the OpenTelemetry Operator, the Collector, the Instrumentation CR, and the demo app — is deployed automatically by Argo CD


## 📁 Repository Structure

```shell
argocd/
  app-of-apps.yaml
  applicationset-platform-helm.yaml
  applicationset-platform.yaml
  applicationset-apps.yaml

platform/
  cert-manager/
  opentelemetry-operator/
  collector/

apps/
  demo/

environments/
  dev/
    values/
      platform-values.yaml
      apps-values.yaml
```

This structure is intentionally simple, scalable, and GitOps‑friendly.
It supports multi‑environment deployments (dev/staging/prod) with clean separation between:
- Platform components (cert-manager, Operator, Collector)
- Application workloads (demo-dotnet)
- Environment‑specific overrides (environments/dev/values/)

## 🧪 Demo Application
The demo-dotnet application is a minimal .NET service used to demonstrate automatic instrumentation via the OpenTelemetry Operator.

It requires no code changes to emit traces the Operator injects the .NET auto‑instrumentation agent at runtime.
Source code lives in:

```shell
src/demo-dotnet/
```

## 🏗️ Building & Publishing the Demo App Image
The demo-dotnet application is not pre‑built.
Before deploying this stack, you must:

- Build the demo app
- Containerize it
- Push the image to your container registry
- Update the image reference in the GitOps config

### Build and push the container image
```shell
docker build -t <your-registry>/demo-dotnet:latest .
docker push <your-registry>/demo-dotnet:latest
```

### Update the image reference

```she;;
apps/demo/values.yaml
```

```yaml
image:
  repository: <your-registry>/demo-dotnet
  tag: latest
```

## 🚀 Deploying This Stack

1. Install Argo CD into your cluster
2. Point Argo CD at this repository
3. Apply the root Application:
   ```shell
   kubectl apply -f argocd/app-of-apps.yaml
   ```
Argo CD will automatically deploy:
- cert-manager
- OpenTelemetry Operator
- Jaeger v2 Collector
- Instrumentation CR
- demo-dotnet application
