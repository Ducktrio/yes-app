import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import { Room } from "@/types/Room";
import { Drawer } from "flowbite-react";
import { useEffect, useState } from "react";

interface Props {
  onChange?: () => void;
  room: Room;
}

export default function DrawerReceptionRoom({ onChange, room }: Props) {
  const roomType = useFetchRoomTypes({ roomId: room.id });
  const [show, setShow] = useState(false);
  useEffect(() => {
    setShow(true);

    return () => {
      setShow(false);
    };
  }, []);

  return (
    <>
      <Drawer open backdrop onClose={() => setShow(false)}></Drawer>
    </>
  );
}
