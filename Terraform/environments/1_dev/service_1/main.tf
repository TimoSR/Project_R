provider "google" {
  project = "project-r-393911"
  region = var.region
}

provider "google-beta" {
  project = "project-r-393911"
}

# Enable the Secret Manager API for the project
resource "google_project_service" "secret_manager" {
  provider = google-beta
  service = "secretmanager.googleapis.com"

  disable_on_destroy = false
}

data "google_secret_manager_secret_version" "secrets" {
  for_each = toset(["MONGODB_DEVELOPMENT_DB", "ENVIRONMENT", "MONGODB_CONNECTION_STRING"])
  secret   = each.key
  version  = "latest"
  depends_on = [ google_project_service.secret_manager ]
}

resource "google_cloud_run_service" "service_x" {
  name     = "service-x" # Replace with your desired service name.
  provider = google
  location = var.region

  metadata {
    annotations =  {
        "run.googleapis.com/ingress" = "all"
    }
  }

  template {
    
    spec {

      containers {

        image = var.container_image

        ports {
          container_port = 8080 # Make sure your application listens on port 8080 inside the container.
        }

        dynamic "env" {
          for_each = data.google_secret_manager_secret_version.secrets
          content {
            name = env.value.secret
            value_from {
              secret_key_ref {
                name = env.value.secret
                key  = "latest"  # Key of the secret in Secret Manager
              }
            }
          }
        }
        
        resources {
          limits = {
            cpu    = var.cpu
            memory = var.memory
          }
        }
      }   
    }
    metadata {
      annotations = {
        "autoscaling.knative.dev/minScale" = "0"
        "autoscaling.knative.dev/maxScale" = "1"
      }
    }
  }
  traffic {
      percent         = 100
      latest_revision = true
  }
}
resource "google_cloud_run_service_iam_member" "all_users" {
  location = var.region
  service  = google_cloud_run_service.service_x.name
  role     = "roles/run.invoker"
  member   = "allUsers"
}