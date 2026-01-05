{{- /* Helper templates for demo chart */ -}}
{{- define "demo.fullname" -}}
{{- printf "%s" .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
