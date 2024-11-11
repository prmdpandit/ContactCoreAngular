export interface Contact {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
}

export interface ContactResponse {
  totalItems: number;
  contacts: Contact[];
}
