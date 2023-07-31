provider "google" {
  project = var.project
  region = var.region
}

provider "google-beta" {
  project = var.project
  region = var.region
}

# Enable the Secret Manager API for the project
resource "google_project_service" "secret_manager" {
  provider = google-beta
  service = "secretmanager.googleapis.com"

  disable_on_destroy = false
}

# Getting the references to secrets
data "google_secret_manager_secret_version" "secrets" {
  for_each = toset(["MONGODB_DEVELOPMENT_DB", "ENVIRONMENT", "MONGODB_CONNECTION_STRING"])
  secret   = each.key
  version  = "latest"
  depends_on = [ google_project_service.secret_manager ]
}

# Creating the Google Cloud Run
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

        # Dynamicaly adding references to the secrets
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
        
        # Setting the amount of cpu and memory
        resources {
          limits = {
            cpu    = var.cpu
            memory = var.memory
          }
        }
      }   
    }

    # Settings for cloud run scaling
    metadata {
      annotations = {
        # Scaling to zero enabled
        "autoscaling.knative.dev/minScale" = "0"
        # Auto Scaling is max 1 container
        "autoscaling.knative.dev/maxScale" = "1"
      }
    }
  }
  traffic {
      percent         = 100
      latest_revision = true
  }
}

# Disabling authentication on microservice for smoke testing 
resource "google_cloud_run_service_iam_member" "all_users" {
  location = var.region
  service  = google_cloud_run_service.service_x.name
  role     = "roles/run.invoker"
  member   = "allUsers"
}