export interface ValidationErrorResponse {
  propertyName: string;
  errorMessage: string;
  attemptedValue: string;
  customState: any | null;
  severity: number;
  errorCode: string;
  formattedMessageArguments: any[];
  formattedMessagePlaceholderValues: {
    MinLength: number;
    MaxLength: number;
    TotalLength: number;
    PropertyName: string;
    PropertyValue: string;
    [key: string]: any;
  };
  resourceName: string | null;
}

export class ValidationError extends Error {
  constructor(public errors: ValidationErrorResponse[]) {
    super("Validation failed");
    this.name = "ValidationError";
    Object.setPrototypeOf(this, ValidationError.prototype);
  }

  getErrors(): ValidationErrorResponse[] {
    return this.errors;
  }

  getMessages(): string[] {
    return this.errors.map((error) => error.errorMessage);
  }
}

