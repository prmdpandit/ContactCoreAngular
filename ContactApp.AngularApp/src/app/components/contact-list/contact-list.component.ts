
import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { Contact, ContactResponse } from '../../models/contact.model';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})

export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];
  totalItems = 0;
  search = '';
  sortBy = 'firstName';
  page = 1;
  pageSize = 10;

  constructor(private contactService: ContactService) { }

  ngOnInit(): void {
    this.loadContacts();
  }

  // Load contacts with search, sorting, and pagination
  loadContacts(): void {
    this.contactService.getContacts(this.search, this.sortBy, this.page, this.pageSize).subscribe(
      (response: ContactResponse) => {
        this.contacts = response.contacts;
        this.totalItems = response.totalItems;
      },
      (error) => {
        console.error('Error loading contacts', error);
      }
    );
  }

  // Delete a contact
  deleteContact(id: number): void {
    this.contactService.deleteContact(id).subscribe(
      () => {
        this.contacts = this.contacts.filter(contact => contact.id !== id);
        this.totalItems -= 1;
      },
      (error) => {
        console.error('Error deleting contact', error);
      }
    );
  }
}
