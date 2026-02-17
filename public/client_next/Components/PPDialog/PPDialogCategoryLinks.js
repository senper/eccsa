import React from 'react';
import Link from 'next/link';
import { Card } from 'react-bootstrap';
import { getDialogList } from '../../api/resources';

export default function DialogCategoryLinks({ getLocal, getLocalByVal }) {
  const [dialogCategories, setDialogCategories] = React.useState([]);
  React.useEffect(() => {
    getDialogList().then((data) => {
      setDialogCategories(data.data.$values);
    });
  }, []);

  return (
    <Card text="white" style={{ width: '18rem' }}>
      <Card.Header style={{ textAlign: 'center', backgroundColor: 'green' }}>
        {getLocal('Issues')}
      </Card.Header>
      <Card.Body>
        <Card.Text>
          {dialogCategories.map((dialog) => (
            <li key={dialog.id} title={dialog.title} className="li-links">
              <Link href={`/ppDialog?categoryId=${dialog.id}`} replace>
                {getLocal(dialog.title)}
              </Link>
            </li>
          ))}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}
