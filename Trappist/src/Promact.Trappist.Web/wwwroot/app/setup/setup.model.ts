export class ConnectionString {
    value: string;
}

export class EmailSettings {
    server: string;
    port: number;
    userName: string;
    password: string;
    connectionSecurityOption: string;
}

export class RegistrationFields {
    name: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export class BasicSetup {
    connectionString: ConnectionString;
    registrationFields: RegistrationFields;
    emailSettings: EmailSettings;
}

export class ServiceResponse {
    isSuccess: boolean;
    exceptionMessage: string;
}