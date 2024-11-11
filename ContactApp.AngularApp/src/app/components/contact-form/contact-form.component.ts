
import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent implements OnInit {
  contactForm: FormGroup;
  public isEditMode = false; 
  contactId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService,
    public router: Router, 
    private route: ActivatedRoute
  ) {
    this.contactForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    // Check if editing an existing contact
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.contactId = +id;
        //this.loadContact();
      }
    });
  }

  //loadContact(): void {
  //  if (this.contactId) {
  //    this.contactService.getContactById(this.contactId).subscribe(contact => {
  //      this.contactForm.patchValue(contact);
  //    });
  //  }
  //}

  onSubmit(): void {
    if (this.contactForm.valid) {
      const contact: Contact = this.contactForm.value;
      if (this.isEditMode && this.contactId) {
        // Update contact
        this.contactService.updateContact(contact).subscribe(() => {
          this.router.navigate(['/contacts']);
        });
      } else {
        // Create new contact
        this.contactService.createContact(contact).subscribe(() => {
          this.router.navigate(['/']);
        });
      }
    }
  }
}
