export const serviceForm=
[
    {
name:"Reference No ", placeholder:"Reference No ", label:"Reference No ", 
required:"true",type:"text" ,options:[]},
{
    name:"date ", placeholder:"Date ", label:"Date ", 
    required:"true",type:"date",options:[]
},
{
    
    name:"PaymentTerm ", placeholder:"Payment Term ", label:"Payment Term ", 
    required:"true",type:"text",options:[]
},
{
    name:"Shipment Term ", placeholder:"Shipment Term ", label:"Shipment Term ", 
    required:"true",type:"email",options:[]
},
{
    name:"Gross Weight ", placeholder:"Gross Weight ", label:"Gross Weight", 
    required:"true",type:"text",options:[]
},
{
    name:"Net Weight ", placeholder:"Net Weight", label:"Net Weight", 
    required:"true",type:"text",options:[]
},
{
    name:"Country ", placeholder:"Country ", label:"Country ", 
    required:"true",type:"select", options:[
        {
            value:"Option 1",
            key:"xxx"
        },
        {
            value:"Option 2",
            key:"xxx"
        },
        {
            value:"Option 3",
            key:"xxx"
        }
    ]
},
{
    name:"City ", placeholder:"City", label:"City", 
    required:"true",type:"text",options:[]
},
{
    name:"Beneficiary Bank ", placeholder:"Beneficiary Bank ", label:"Beneficiary Bank ", 
    required:"true",type:"select", options:[
        {
            value:"CBE",
            key:"xxx"
        },
        {
            value:"Dashin",
            key:"xxx"
        },
        {
            value:"Zemen",
            key:"xxx"
        }
    ]
},
{
    name:"Attachment ", placeholder:"Attachment ", label:"Attachment", 
    required:"true",type:"file",options:[]
}

  
]