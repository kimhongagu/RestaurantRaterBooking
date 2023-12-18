<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class ChiTietDonHang extends Model
{
    protected $table = 'chitietdonhang'; // Tên bảng trong cơ sở dữ liệu

    protected $fillable = [
        'donhang_id', 
        'sach_id', 
        'vpp_id', 
        'soluong', 
        'giasanpham', 
        'thanhtien', 
    ];

    public function DonHang()
    {
        return $this->belongsTo(DonHang::class, 'donhang_id', 'id');
    }

    public function Sach()
    {
        return $this->belongsTo(Sach::class, 'sach_id', 'id');
    }

    public function VanPhongPham()
    {
        return $this->belongsTo(VanPhongPham::class, 'vpp_id', 'id');
    }
}
